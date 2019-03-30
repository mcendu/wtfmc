using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace wtfmc
{
    public static class Downloader
    {
        public static readonly HttpClient hclient = new HttpClient();
        private static readonly ILog log = LogManager.GetLogger(typeof(Downloader));

        /// <summary>
        /// Download a file.
        /// </summary>
        /// <param name="dl">The download data.</param>
        public static async Task DownloadAsync(Download dl)
        {
            // Initialize variables
            log.Info($"Downloading {dl.Src.AbsoluteUri}");
            Stream from = await hclient.GetStreamAsync(dl.Src);
            FileStream to = new FileStream(dl.Path, FileMode.Create, FileAccess.ReadWrite);
            int readlen;
            byte[] buf = new byte[2048];

            // Concurrently move data from download stream to file
            while ((readlen = from.Read(buf, 0, 2048)) != 0)
            {
                to.Write(buf, 0, 2048);
            }
            to.Flush();
            to.Position = 0;

            // Check the hash of the download
            if (!Util.checkIntegrity(to, dl.Hash))
            {
                log.Error($"Failed to download {dl.Src.AbsoluteUri}");
                throw new Exception(); // Fault the task
            }
            log.Info($"Downloaded {dl.Src.AbsoluteUri}");
        }
    }

    /// <summary>
    /// Represents a file and related data.
    /// </summary>
    public class Download
    {
        public Uri Src { get; }

        public string Path { get; }

        public string Hash { get; }

        public Download(Uri src, string path, string hash)
        {
            Src = src;
            Path = path;
            Hash = hash;
        }

        public Download(string src, string path, string hash)
        {
            Src = new Uri(src);
            Path = path;
            Hash = hash;
        }

        /// <summary>
        /// Check the integrity of a file.
        /// </summary>
        public bool Check()
        {
            try
            {
                return Util.checkIntegrity(File.Open(Path, FileMode.Open), Hash);
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}
