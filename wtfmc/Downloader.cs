using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace wtfmc
{
    public class Downloader
    {
        private readonly HttpClient hclient = new HttpClient();
        private TaskFactory factory = new TaskFactory();
        public readonly short maxThreads;
        public Queue<Download> downQueue = null;
        private static readonly ILog log = LogManager.GetLogger(typeof(Downloader));
        public bool qend = false;
        
        public Downloader(short threads)
        {
            maxThreads = threads;
        }

        public Downloader() => new Downloader(8);

        /// <summary>
        /// Download multiple files concurrently.
        /// 并发式下载多个文件。
        /// </summary>
        /// <param name="q"></param>
        public async Task DownloadAsync() => await Task.Run(delegate ()
        {
            if (downQueue == null) downQueue = new Queue<Download>();
            Download pend;
            Task[] tlist = new Task[maxThreads];
            while (!qend || downQueue.Count != 0)
            {
                for (int j = 0; j < maxThreads; j++)
                {
                    if (tlist[j] == null || tlist[j].Status != TaskStatus.Running)
                    {
                        pend = downQueue.Dequeue();
                        tlist[j] = factory.StartNew(delegate (object input)
                        {
                            // Initialize variables
                            Download dl = input as Download;
                            log.Info($"Downloading {dl.from.AbsoluteUri}");
                            Stream from = hclient.GetStreamAsync(dl.from).Result;
                            FileStream to = new FileStream(dl.to, FileMode.Create, FileAccess.ReadWrite);
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
                                log.Warn($"Failed to download {dl.from.AbsoluteUri}; Requeueing");
                                downQueue.Enqueue(dl);
                            }
                            else
                            {
                                log.Info($"Downloaded {dl.from.AbsoluteUri}");
                            }
                        }, pend);

                        // When download queue is empty
                        if (qend && downQueue.Count == 0)
                        {
                            Task.WaitAll(tlist);
                            break;
                        }
                        while (!qend && downQueue.Count == 0) ;
                    }
                }
            }
        });
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

        public Download(Uri from, string to, string hash, long length)
        {
            this.from = from;
            this.to = to;
            this.hash = hash;
            this.length = length;
        }
    }
}
