using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    /// <summary>
    /// Parts of the version.json that
    /// are mosly consistent across
    /// versions.
    /// </summary>
    public abstract class Version : IVersion
    {
        public Version(JObject vdata)
        {
            this.vdata = vdata;
        }

        /// <summary>
        /// Automatically determine the version type
        /// of the result.
        /// </summary>
        /// <param name="obj">The JObject.</param>
        public static Version Parse(JObject obj)
        {
            if (obj.ContainsKey("minimumLauncherVersion")) {
                switch ((int)obj["minimunLauncherVersion"])
                {
                    case 21:
                        return new Version21(obj);
                    default:
                        return null;
                }
            }
            return null;
        }

        public static Version Parse(string obj)
        {
            return Parse(JObject.Parse(obj));
        }

        public Profile Profile { get; set; }

        protected internal string SetCD()
        {
            string ret = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Profile.GameDir);
            return ret;
        }

        protected JObject vdata;

        protected string VID => (string)vdata["id"];

        protected readonly IDownloadSource source;

        public ILoginClient Login { get; set; }

        public void CheckClient()
        {
            throw new NotImplementedException();
        }
        
        public void UnpackNatives()
        {
            throw new NotImplementedException();
        }

        public void CheckAssetsIndex()
        {
            string o = SetCD();
            Download[] dl = { new Download((string)vdata["assetIndex"]["url"],
                $"assets/indexes/{(string)vdata["assetIndex"]["id"]}",
                (string)vdata["assetIndex"]["hash"]) };
            Util.CheckFiles(dl);
            // Load Assets.
            AssetsIndex assets = new AssetsIndex(JObject.Parse(File.ReadAllText($"assets/indexes/{(string)vdata["assetIndex"]["id"]}")));
            assets.checkAssets();
            Directory.SetCurrentDirectory(o);
        }

        protected Hashtable GenParamHash() => new Hashtable
            {
                { "auth_player_name", Login.Username },
                { "version_name", "vanilla" },
                { "game_directory", Profile.GameDir },
                { "assets_root", $"{Profile.GameDir}/assets" },
                { "assets_index_name", vdata["assetIndex"]["id"] },
                { "auth_uuid", Login.ID },
                { "auth_access_token", Login.AccessToken },
                { "user_type", "mojang" },
                { "version_type", vdata["type"] },
                { "resolution_width", Profile.Width },
                { "resolution_height", Profile.Height }
            };

        /// <summary>
        /// Construct the -Xmx parameter.
        /// </summary>
        /// <returns>The max heap size allowed in the form of "-Xmx1024M".</returns>
        protected string GenXmx()
        {
            // TODO.
            // Intends to check for total memory 
            // and use the lower of 2G and half
            // of total mem. However, there is
            // no cross-platform way to check
            // memory, so 2G only.
            return "-Xmx2G";
        }

        public abstract void CheckLibraries();
        public abstract string GenerateClasspath();
        public abstract List<string> GenerateArgs(ILoginClient login, Profile profile);
        public abstract List<string> GenerateVMArgs(ILoginClient login, Profile profile);
    }
}
