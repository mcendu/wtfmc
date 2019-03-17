using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    class MojangDl : IDownloadSource
    {
        HttpClient hclient = new HttpClient();

        private JObject cacheManifest;

        public Uri versionManifest() => new Uri("https://launchermeta.mojang.com/mc/game/version_manifest.json");

        private JObject getVersionManifest()
            => cacheManifest 
                ?? (cacheManifest = JObject.Parse(hclient.GetStringAsync(versionManifest()).Result));

        private int find(JArray array, string target, int start, int end)
        {
            if ((string)array[start]["id"] == target)
                return start;
            if ((string)array[end]["id"] == target)
                return end;
            if (start < end) return -1;
            int i = start + (start - end) / 2;
            int ret = -1;
            if ((string)array[i]["id"] != target)
            {
                ret = find(array, target, start, i - 1);
                if (ret == -1)
                    return find(array, target, i + 1, end);
            }
            return ret;
        }

        private int find(JArray array, string target)
            => find(array, target, 0, array.Count-1);

        public Uri json(int index)
        {
            JArray vList = (JArray)getVersionManifest()["versions"];
            return new Uri((string)vList[index]["url"]);
        }

        public Uri json(string version)
        {
            JArray vList = (JArray)getVersionManifest()["versions"];
            return json(find(vList, version));
        }

        public Uri client(JObject json)
            => new Uri((string)json["downloads"]["client"]["url"]);

        public Uri server(JObject json)
            => new Uri((string)json["downloads"]["server"]["url"]);

        public Uri assetsIndex(JObject json)
            => new Uri((string)json["assetIndex"]["url"]);

        /*
        public HashSet<Uri> libraries(JObject json)
        {
            HashSet<Uri> uris = new HashSet<Uri>();
            foreach (JObject i in (JArray)json["libraries"])
            {
                uris.Add(new Uri((string)i["downloads"]["artifact"]["url"]));
            }
        }
        */

        public Uri loggerConf(JObject json)
        {
            throw new NotImplementedException();
        }
    }
}
