using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json.Linq;
using wtfmc.Config;

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
                switch ((int)obj["minimumLauncherVersion"])
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
                (string)vdata["downloads"]["client"]["sha1"]) };
            Util.CheckFiles(dl);
            Directory.SetCurrentDirectory(o);
        }
        
        // 
        /*
        public void UnpackNatives(string path)
        {
            string o = SetCurrentDirectory(path);
            string tmp = $"{Util.TEMP}/wtfmc/dummy";
            Util.GenDir($"versions/{VID}/{VID}-natives/dummy");
            Util.GenDir(tmp);
            RuleReader rr = new RuleReader();
            foreach (JObject i in vdata["libraries"])
            {
                rr.Execute(new Func<JArray>(() =>
                {
                    if (i.ContainsKey("rules"))
                        return (JArray)i["rules"];
                    return null;
                })(),
                () =>
                {
                    Directory.SetCurrentDirectory(path);
                    if (i.ContainsKey("natives"))
                    {
                        // Extract files
                        ZipFile.ExtractToDirectory(
                            $"libraries/{(string)i["downloads"]["classifiers"]["natives-windows"]["path"]}",
                            tmp);
                        // Remove META-INF
                        Util.RmR($"{tmp}/META-INF");
                        // Move files
                        foreach (string j in Directory.EnumerateFileSystemEntries(tmp))
                        {
                            try
                            {
                                Directory.Move(Path.Combine(tmp, j), $"versions/{VID}/{VID}-natives/");
                            }
                            catch (IOException)
                            {

                            }
                        }
                        Util.RmR(tmp);
                    }
                    Directory.SetCurrentDirectory(o);
                });
                Directory.SetCurrentDirectory(o);
            }
        }
        */

        private void CheckAssetsIndex(string path)
        {
            string o = SetCurrentDirectory(path);
            Download[] dl = { new Download((string)vdata["assetIndex"]["url"],
                $"assets/indexes/{(string)vdata["assetIndex"]["id"]}.json",
                (string)vdata["assetIndex"]["sha1"]) };
            Util.CheckFiles(dl);
            // Load Assets.
            AssetsIndex assets = new AssetsIndex(JObject.Parse(File.ReadAllText($"assets/indexes/{(string)vdata["assetIndex"]["id"]}.json")));
            assets.checkAssets();
            Directory.SetCurrentDirectory(o);
        }

        protected Hashtable GenParamHash(ILoginClient login, Profile profile) => new Hashtable
            {
                { "auth_player_name", login.Username },
                { "version_name", "vanilla" },
                { "game_directory", profile.GameDir },
                { "assets_root", $"{profile.GameDir}/assets" },
                { "assets_index_name", (string)vdata["assetIndex"]["id"] },
                { "auth_uuid", login.ID },
                { "auth_access_token", login.AccessToken },
                { "user_type", "mojang" },
                { "version_type", (string)vdata["type"] },
                { "resolution_width", profile.Width },
                { "resolution_height", profile.Height },
                { "natives_directory", $"{VID}/{VID}-natives" },
                { "launcher_name", "WTFMC" },
                { "launcher_version", "" },
                { "classpath", GenerateClasspath() }
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
        public abstract List<string> GenerateVMArgs(ILoginClient login, Profile profile);
    }
}
