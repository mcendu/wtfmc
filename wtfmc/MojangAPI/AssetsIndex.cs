﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    public class AssetsIndex : IAssetsIndex
    {
        public AssetsIndex() { }

        public AssetsIndex(JObject j)
        {
            Assets = j;
        }

        public JObject Assets { get; }

        public void checkAssets()
        {
            Util.GenDir("assets/objects");
            Directory.SetCurrentDirectory("assets/objects");
            foreach (JProperty i in Assets["objects"])
            {
                string dest = (string)i.Value["hash"];
                string dir = dest.Substring(0, 2);
                Download dl = new Download($"http://resources.download.minecraft.net/{dir}/{dest}", $"{dir}/{dest}", dest);
                Util.CheckFiles(new Download[] { dl });
            }
            Directory.SetCurrentDirectory("../..");
        }
    }
}
