﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    /// <summary>
    /// Minecraft JE 1.13 version format, identified with minimunLauncherVersion 21.
    /// </summary>
    public sealed class Version21 : Assets
    {
        public Version21()
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
                // Libs with natives have two repeating entries.
                // Per above, if an entry has natives, ignore its "artifact" part.
                if (i.ContainsKey("natives"))
                {
                    // TODO: Once it can use .NET Core, replace with an OS detector.
                    string dlTo = i["downloads"]["classifiers"]["natives-windows"]["path"].ToString();
                    FileQ.Enqueue(genDlData(i["downloads"]["classifiers"]["natives-windows"], $"libraries/{dlTo}"));
                }
                else
                {
                    string dlTo = i["downloads"]["artifact"]["path"].ToString();
                    FileQ.Enqueue(genDlData(i["downloads"]["artifact"], $"libraries/{dlTo}"));
                }
            }
        }
    }
}