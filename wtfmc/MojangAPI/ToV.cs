using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    /// <summary>
    /// Mojang version manifest.
    /// </summary>
    public sealed class ToV : IToV
    {
        public ToV(string tov)
        {
            this.tov = JObject.Parse(tov);
        }

        /// <summary>
        /// Download the official list.
        /// </summary>
        /// <param name="source">The download source.</param>
        public static ToV Download(IDownloadSource source)
        {
            string dlsrc = source.Translate("https://launchermeta.mojang.com/mc/game/version_manifest.json");
            string tov = Downloader.hclient.GetStringAsync(dlsrc).Result;
            return new ToV(tov);
        }

        private readonly JObject tov;

        public string GameDir { get; set; }

        public IVersion GetLatest(IDownloadSource dlsrc)
            => GetVersion((string)tov["latest"]["release"], dlsrc);

        public IVersion GetLatestSnap(IDownloadSource dlsrc)
            => GetVersion((string)tov["latest"]["snapshot"], dlsrc);

        public IVersion GetVersion(string identifier, IDownloadSource dlsrc)
        {
            var version = from v in (JArray)tov["versions"]
                          where (string)v["id"] == identifier
                          select v["url"];
            string vdata =
                Downloader.hclient.GetStringAsync(
                    dlsrc.Translate((string)version.First())
                    )
                    .Result;
            Util.GenDir($"{GameDir}/versions/{identifier}");
            using (FileStream f = File.Open($"{GameDir}/versions/{identifier}/{identifier}.json", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(f))
            {
                sw.Write(vdata);
            }
            return Version.Parse(vdata);
        }
    }
}
