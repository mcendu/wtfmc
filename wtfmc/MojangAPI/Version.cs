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

        // A shell on Dir.SetCD
        protected internal string SetCurrentDirectory(string path)
        {
            string ret = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(path);
            return ret;
        }

        protected JObject vdata;

        public string MainClass => (string)vdata["mainClass"];

        protected string VID => (string)vdata["id"];

        protected readonly IDownloadSource source;

        public ILoginClient Login { get; set; }

        public void CheckData(string path)
        {
            CheckClient(path);
            CheckLibraries(path);
            CheckAssetsIndex(path);
        }

        private void CheckClient(string path)
        {
            string o = SetCurrentDirectory(path);
            Download[] dl = { new Download((string)vdata["downloads"]["client"]["url"],
                $"versions/{VID}/{VID}.jar",
                (string)vdata["downloads"]["client"]["hash"]) };
            Util.CheckFiles(dl);
            Directory.SetCurrentDirectory(o);
        }
        
        public void UnpackNatives()
        {
            throw new NotImplementedException();
        }

        private void CheckAssetsIndex(string path)
        {
            string o = SetCurrentDirectory(path);
            Download[] dl = { new Download((string)vdata["assetIndex"]["url"],
                $"assets/indexes/{(string)vdata["assetIndex"]["id"]}",
                (string)vdata["assetIndex"]["hash"]) };
            Util.CheckFiles(dl);
            // Load Assets.
            AssetsIndex assets = new AssetsIndex(JObject.Parse(File.ReadAllText($"assets/indexes/{(string)vdata["assetIndex"]["id"]}")));
            assets.checkAssets();
            Directory.SetCurrentDirectory(o);
        }

        protected Hashtable GenParamHash(Profile profile) => new Hashtable
            {
                { "auth_player_name", Login.Username },
                { "version_name", "vanilla" },
                { "game_directory", profile.GameDir },
                { "assets_root", $"{profile.GameDir}/assets" },
                { "assets_index_name", vdata["assetIndex"]["id"] },
                { "auth_uuid", Login.ID },
                { "auth_access_token", Login.AccessToken },
                { "user_type", "mojang" },
                { "version_type", vdata["type"] },
                { "resolution_width", profile.Width },
                { "resolution_height", profile.Height }
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

        protected abstract void CheckLibraries(string path);

        /// <remarks>
        /// Can be implemented here, but for
        /// optimization purposes (warai)...
        /// </remarks>
        public abstract string GenerateClasspath();
        public abstract List<string> GenerateArgs(ILoginClient login, Profile profile);
        public abstract List<string> GenerateVMArgs();
    }
}
