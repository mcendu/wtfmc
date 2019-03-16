using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    /// <summary>
    /// Minecraft JE assets format.
    /// </summary>
    public abstract class Assets : VersionLoader
    {
        protected Download genDlData(JToken json, string dlTo, Uri dlFrom)
            => new Download(
                dlFrom,
                dlTo,
                json["sha1"].ToString(),
                json["size"].Value<int>()
            );

        protected Download genDlData(JToken json, string dlTo, string dlFrom)
            => genDlData(json, dlTo, new Uri(dlFrom));

        protected Download genDlData(JToken json, string dlTo)
            => genDlData(json, dlTo, new Uri(json["url"].ToString()));

        public override void LoadAssetsIndex(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            JObject index = (JObject) JObject.Parse(File.ReadAllText(path)) ["objects"];
            foreach (JToken i in index.Children())
            {
                string hash = (string)i["hash"];
                string dir = hash.Substring(0, 2);
                FileQ.Enqueue(genDlData(i, $"assets/objects/{dir}/{hash}", $"http://resources.download.minecraft.net/{dir}/{hash}"));
            }
            throw new NotImplementedException();
        }
    }
}
