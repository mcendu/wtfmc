using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc.Tasks
{
    public class FileDownloadTask : Task
    {
        private static readonly HttpClient hclient = new HttpClient();

        public FileDownloadTask(Uri url, string path, string hash)
            : base(async () =>
            {
                HttpResponseMessage res;
                long progress;
                int r;
                long len;
                string temppath = "";
                Exception exception;

                for (int tries = 0; tries < 5; tries++)
                    try
                    {
                        progress = 0;
                        res = await hclient.GetAsync(url);
                        if (!res.IsSuccessStatusCode)
                        {
                            throw new Exception($"Cannot download file {url.ToString()}: {res.StatusCode.ToString()}");
                        }
                        len = long.Parse(res.Headers.GetValues("Content-Length").First());
                        temppath = Path.GetTempFileName();

                        using (HashAlgorithm hasher = new SHA1Managed())
                        {
                            using (FileStream file = File.Open(
                                temppath,
                                FileMode.Create))
                            {
                                using (Stream remote = await res.Content.ReadAsStreamAsync())
                                {
                                    hasher.Initialize();
                                    byte[] buffer = new byte[1024];
                                    while (true)
                                    {
                                        r = remote.Read(buffer, 0, 512);
                                        if (r == 0)
                                        {
                                            hasher.TransformFinalBlock(buffer, 0, 0);
                                            break;
                                        }
                                        hasher.TransformBlock(buffer, 0, r, buffer, 0);

                                        file.Write(buffer, 0, r);
                                        progress += r;
                                    }
                                }
                            }

                            // Move files.
                            File.Delete(path);
                            Util.GenDir(Path.GetDirectoryName(path));
                            try
                            {
                                // Clear temp attribute
                                File.SetAttributes(temppath, File.GetAttributes(temppath) & ~FileAttributes.Temporary);
                                File.Move(temppath, path);
                            }
                            catch (Exception ex)
                            {
                                throw new IOException($"Unable to move file to {path}", ex);
                            }

                            if (progress != len)
                                throw new Exception($"Bad file size, downloaded {progress}, expected {len}");

                            // Check integrity
                            if (hash != null && Util.Bintohex(hasher.Hash) != hash)
                                throw new Exception($"Checksum mismatch, found {Util.Bintohex(hasher.Hash)}, expected {hash}");
                        }
                    }
                    catch (Exception e)
                    {
                        if (File.Exists(temppath))
                            File.Delete(temppath);
                        exception = e;
                    }
            })
        {
            Url = url;
            FilePath = path;
            Hash = hash;
        }

        public Uri Url { get; }
        public string FilePath { get; }
        public string Hash { get; }

        public FileDownloadTask(string url, string filename, string hash)
            : this(new Uri(url), filename, hash)
        {

        }
    }
}
