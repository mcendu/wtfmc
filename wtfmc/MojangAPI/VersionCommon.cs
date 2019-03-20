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
    /// Parts of the version.json that
    /// are mosly consistent across
    /// versions.
    /// </summary>
    public abstract class VersionCommon : IVersionParser
    {
        Downloader dl = new Downloader();

        public VersionCommon(JObject vdata)
        {
            Version = vdata;
            VID = (string)Version["id"];
        }

        public string GameDir
        {
            get => Directory.GetCurrentDirectory();
            set => Directory.SetCurrentDirectory(value);
        }

        public JObject Version { get; }

        public string VID { get; }

        public IDownloadSource Source { get; set; }

        public ILoginClient Login { get; set; }

        protected void checkFiles(IEnumerable<Download> filedata)
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
                    dl.DownloadAsync(i).Wait();
            }
        }

        public void checkClient()
        {
            throw new NotImplementedException();
        }
        
        public void unpackNatives()
        {
            throw new NotImplementedException();
        }

        public void checkAssetsIndex()
        {
            Download[] dl = { new Download(Source.assetsIndex(Version),
                $"assets/indexes/{(string)Version["assetIndex"]["id"]}",
                (string)Version["assetIndex"]["hash"]) };
            checkFiles(dl);
        }

        public abstract void checkLibraries();
        public abstract string generateClasspath();
        public abstract List<string> generateArgs();
        public abstract List<string> generateVMArgs();
    }
}
