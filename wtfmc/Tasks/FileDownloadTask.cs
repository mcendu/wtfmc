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
        public static readonly HttpClient hclient = new HttpClient();

        public FileDownloadTask(Uri url, string path, string hash)
            : base(async () =>
            {
                // Get data
                HttpResponseMessage res = await hclient.GetAsync(url);
                if (!res.IsSuccessStatusCode)
                {
                    throw new Exception($"Cannot download file {url.ToString()}: {res.StatusCode.ToString()}");
                }

                long progress = 0;
                int r;
                long len = long.Parse(res.Headers.GetValues("Content-Length").First());
                string temppath = Path.GetTempFileName();

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
                        File.Move(temppath, path);
                    } catch (Exception ex)
                    {
                        throw new IOException($"Unable to move file to {path}", ex);
                    }

                    if (progress != len)
                        throw new Exception($"File size is not correct");

                    // Check integrity
                    if (hash != null && Util.Bintohex(hasher.Hash) != hash)
                        throw new Exception($"Hash not correct");
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
