using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace wtfmc
{
    public class Downloader
    {
        /// <summary>
        /// The HTTP client.
        /// HTTP客户端。
        /// </summary>
        private readonly HttpClient hclient;
        private static readonly ILog log = LogManager.GetLogger(typeof(Downloader));
        
        public Downloader()
        {
            hclient = new HttpClient();
        }

        /// <summary>
        /// Download multiple files concurrently.
        /// 并发式下载多个文件。
        /// </summary>
        /// <param name="q"></param>
        public void Download(Queue<Download> q)
        {
            Parallel.ForEach(q, delegate (Download i)
            {
                log.Info($"Downloading {i.from.AbsoluteUri}");
                FileStream to = new FileStream(i.to, FileMode.Create, FileAccess.ReadWrite);
                int proggain;
                byte[] buffer = new byte[1024];
                Stream from = hclient.GetStreamAsync(i.from).Result;
                // Concurrently move data across streams
                while ((proggain = from.Read(buffer, 0, 1024)) != 0)
                {
                    i.progress += proggain;
                    to.Write(buffer, 0, 1024);
                }
                // TODO: integrity check
                log.Info($"Finished download of {i.from.AbsoluteUri}");
            });
        }
    }

    /// <summary>
    /// Represents a file to be downloaded.
    /// 一个需要下载的文件。
    /// </summary>
    public class Download
    {
        public readonly Uri from;
        public readonly string to;
        public readonly string hash;
        public readonly long length;
        public long progress;

        public Download(Uri from, string to, string hash, long length)
        {
            this.from = from;
            this.to = to;
            this.hash = hash;
            this.length = length;
        }
    }
}
