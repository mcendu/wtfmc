using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc
{
    public class Downloader
    {
        public readonly Download target;
        public readonly Task task;

        public Downloader(Download target)
        {
            this.target = target;
        }

        public void TaskBody()
        {
            HttpClient hclient = new HttpClient();
            FileStream file = new FileStream(target.toPath, FileMode.Create);
            Task<Stream> remote = hclient.GetStreamAsync(target.path);
        }
    }
    
    /// <summary>
    /// A downloadable object.
    /// </summary>
    public class Download
    {
        public Download(string hash, Uri path)
        {
            this.hash = hash ?? throw new ArgumentNullException(nameof(hash));
            this.path = path;
        }
        public Download(string hash, string path) => new Download(hash, new Uri(path));

        /// <summary>
        /// The hash for the resource.
        /// </summary>
        public readonly string hash;
        /// <summary>
        /// The URI of the resource.
        /// </summary>
        public readonly Uri path;
        /// <summary>
        /// The path the download would go to.
        /// </summary>
        public string toPath;
    }
}
