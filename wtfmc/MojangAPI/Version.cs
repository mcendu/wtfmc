using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    /// <summary>
    /// Parts of the version.json that
    /// are mosly consistent across
    /// versions.
    /// </summary>
    public abstract class Version : IVersionParser
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

        protected void CheckFiles(IEnumerable<Download> filedata)
        {
            foreach (Download i in filedata)
            {
                string d = Directory.GetCurrentDirectory();
                string[] compo = i.Path.Split(new char[] { Path.DirectorySeparatorChar });
                foreach (string j in compo)
                {
                    d = Path.Combine(d, j);
                    if (!Directory.Exists(d))
                    {
                        Directory.CreateDirectory(d);
                    }
                }
                if (new Func<bool>(() => {
                    try
                    {
                        FileStream f = File.Open(i.Path, FileMode.Open);
                        return Util.checkIntegrity(f, i.Hash);
                    }
                    catch (IOException)
                    {
                        return false;
                    }
                })())
                    Downloader.DownloadAsync(i).Wait();
            }
        }

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
            CheckFiles(dl);
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

        public abstract void CheckLibraries();
        public abstract string GenerateClasspath();
        public abstract List<string> GenerateArgs();
        public abstract List<string> GenerateVMArgs();
    }
}
