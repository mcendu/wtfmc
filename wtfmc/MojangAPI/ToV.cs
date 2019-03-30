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

        private readonly JObject tov;
        public IDownloadSource DlSrc { get; set; }

        public Profile Profile { get; set; }

        public IVersionParser GetLatest()
            => GetVersion((string)tov["latest"]["release"]);

        public IVersionParser GetLatestSnap()
            => GetVersion((string)tov["latest"]["snapshot"]);

        public IVersionParser GetVersion(string identifier)
        {
            var version = from v in (JArray)tov["versions"]
                          where (string)v["id"] == identifier
                          select v["url"];
            string vdata =
                Downloader.hclient.GetStringAsync(
                    DlSrc.Translate((string)version.First())
                    )
                    .Result;
            File.WriteAllText($"{Profile.GameDir}/versions/{identifier}/{identifier}.json", vdata);
            return Version.Parse(vdata);
        }
    }
}
