using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace wtfmc
{
    /// <summary>
    /// Loads version data.
    /// </summary>
    public abstract class VersionLoader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VersionLoader));

        public Queue<Download> FileQ { get; set; }

        /// <summary>
        /// Load and parse a version.json, and queue files for checking.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        public abstract void LoadVersionIndex(string path);

        /// <summary>
        /// Load and parse an assets.json, and queue files for checking.
        /// </summary>
        /// <param name="path"></param>
        public abstract void LoadAssetsIndex(string path);

        /// <summary>
        /// Check a queue of files, and download them if they fail the check.
        /// </summary>
        public void CheckFiles()
        {
            int chkLen = FileQ.Count;
            FileStream f;
            Download path;
            for (int i=0; i<chkLen; i++)
            {
                path = FileQ.Dequeue();
                try
                {
                    f = new FileStream(path.to, FileMode.Open, FileAccess.Read);
                    if (!Util.checkIntegrity(f, path.hash))
                    {
                        log.Debug($"Queued {path.from.AbsoluteUri} for redownloading");
                        FileQ.Enqueue(path);
                    }
                }
                catch (FileNotFoundException)
                {
                    log.Debug($"Queued {path.from.AbsoluteUri} for downloading");
                    FileQ.Enqueue(path);
                }
            }
            // Download files that failed the check
            Downloader dlr = new Downloader();
            dlr.downQueue = FileQ;
            try
            {
                dlr.DownloadAsync().Wait();
            }
            catch (Exception e)
            {
                log.Error("An error occurred while downlooading");
                throw new System.Net.WebException("An error downloaded while downloading", e);
            }
        }
    }
}
