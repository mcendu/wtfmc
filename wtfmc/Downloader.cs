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
    public class Downloader
    {
        private readonly HttpClient hclient = new HttpClient();
        private static readonly ILog log = LogManager.GetLogger(typeof(Downloader));
        
        public Downloader()
        {
        }

        /// <summary>
        /// Download a file.
        /// </summary>
        /// <param name="dl">The download data.</param>
        public async Task DownloadAsync(Download dl)
        {
            // Initialize variables
            log.Info($"Downloading {dl.src.AbsoluteUri}");
            Stream from = await hclient.GetStreamAsync(dl.src);
            FileStream to = new FileStream(dl.path, FileMode.Create, FileAccess.ReadWrite);
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
            if (!Util.checkIntegrity(to, dl.hash))
            {
                log.Error($"Failed to download {dl.src.AbsoluteUri}");
                throw new Exception(); // Fault the task
            }
            log.Info($"Downloaded {dl.src.AbsoluteUri}");
        }
    }

    /// <summary>
    /// Represents a file and related data.
    /// </summary>
    public class Download
    {
        public readonly Uri src;
        public readonly string path;
        public readonly string hash;
        // public readonly long length;

        public Download(Uri src, string path, string hash)
        {
            this.src = src;
            this.path = path;
            this.hash = hash;
        }

        /// <summary>
        /// Check the integrity of a file.
        /// </summary>
        public bool Check()
        {
            try
            {
                return Util.checkIntegrity(File.Open(path, FileMode.Open), hash);
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}
