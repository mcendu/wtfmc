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
    abstract class VersionCommon : IVersionParser
    {
        Downloader dl = new Downloader();

        public VersionCommon(JObject vdata)
        {
            Version = vdata;
            VID = (string)Version["id"];
        }

        public JObject Version { get; }

        public string VID { get; }

        public IDownloadSource Source { get; set; }

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
        }

        public void checkLibraries()
        {
            RuleReader rr = new RuleReader();
            HashSet<Download> downloads = new HashSet<Download>();
            foreach (JObject i in Version["libraries"])
            {
                rr.Execute(new Func<JObject>(() =>
                {
                    if (i.ContainsKey("rules"))
                        return (JObject)i["rules"];
                    return null;
                })(),
                () =>
                {
                    downloads.Add(new Download(
                        Source.library(i),
                        "libraries/" + (string)i["artifact"]["path"],
                        (string)i["artifact"]["hash"]
                        ));
                    if (i.ContainsKey("natives"))
                    {
                        downloads.Add(new Download(
                            Source.native(i),
                            "libraries/" + (string)i["classifiers"]["natives-windows"]["path"],
                            (string)i["classifiers"]["natives-windows"]["hash"]
                            ));
                    }
                });
            }
            checkFiles(downloads);
        }

        public void checkClient()
        {
            throw new NotImplementedException();
        }

        public string generateClasspath()
        {
            RuleReader rr = new RuleReader();
            string classpath = "";
            foreach (JObject i in Version["libraries"])
            {
                rr.Execute(new Func<JObject>(() =>
                {
                    if (i.ContainsKey("rules"))
                        return (JObject)i["rules"];
                    return null;
                })(),
                () =>
                {
                    if (!i.ContainsKey("natives"))
                        classpath += Path.GetFullPath("libraries/" + (string)i["artifact"]["path"]) + ";";
                });
            }
            classpath += $"versions/{VID}/{VID}.jar";
            return classpath;
        }

        public void genLibraryDirectory()
        {
            throw new NotImplementedException();
        }

        public void unpackNatives()
        {
            throw new NotImplementedException();
        }

        public abstract void checkAssets();
        public abstract List<string> generateArgs();
        public abstract List<string> generateVMArgs();
    }
}
