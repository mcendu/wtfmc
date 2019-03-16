using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    /// <summary>
    /// Minecraft JE 1.8 version format, identified with minimunLauncherVersion 18.
    /// </summary>
    public sealed class Version18 : Assets
    {
        public Version18()
        {
            FileQ = new Queue<Download>();
        }

        public override void LoadVersionIndex(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            string vNo = Path.GetFileNameWithoutExtension(path);
            JObject index = JObject.Parse(File.ReadAllText(path));

            // Queue the asset index
            JToken assetIndex = index["assetIndex"];
            FileQ.Enqueue(genDlData(assetIndex, $"assets/indexes/{new Uri(assetIndex["url"].ToString()).Segments[4]}"));
            // Queue the client jar
            JToken clientJar = index["downloads"]["client"];
            FileQ.Enqueue(genDlData(clientJar, $"versions/{vNo}/{vNo}.jar"));
            // Queue the required libraries
            foreach (JObject i in (JArray)index["libraries"])
            {
                // Exclude all libraries with rules
                // (as currently known, all libraries with rules are Mac specific)
                if (i.ContainsKey("rules")) continue;

                string dlTo = i["downloads"]["artifact"]["path"].ToString();
                FileQ.Enqueue(genDlData(i["downloads"]["artifact"], $"libraries/{dlTo}"));
                // Obtain natives
                if (i.ContainsKey("natives"))
                {
                    // TODO: Once it can use .NET Core, replace with an OS detector.
                    dlTo = i["downloads"]["classifiers"]["natives-windows"]["path"].ToString();
                    FileQ.Enqueue(genDlData(i["downloads"]["classifiers"]["natives-windows"], $"libraries/{dlTo}"));
                }
            }
        }
    }
}
