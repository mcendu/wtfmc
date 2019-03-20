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
    /// Minecraft Java JSON format from 1.13.
    /// Changes from the previous version (1.9) include:
    /// * Minimal launcher version changed to 21 (from 18).
    /// * No longer has a separate LWJGL build for macOS
    /// * Arguments spec moved from minecraftArguments to arguments.game.
    /// * Added another arg spec for JVM.
    /// * Explicit specification of --demo and --width/--height.
    /// * A new convention for libs with natives
    /// ** One entry without natives, one with natives.
    /// </summary>
    public sealed class Version13 : VersionCommon
    {
        public Version13(JObject vdata) : base(vdata)
        {

        }

        public override string generateClasspath()
        {
            RuleReader rr = new RuleReader();
            string classpath = "";
            foreach (JObject i in Version["libraries"])
            {
                rr.Execute(new Func<JArray>(() =>
                {
                    if (i.ContainsKey("rules"))
                        return (JArray)i["rules"];
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

        public override void checkLibraries()
        {
            RuleReader rr = new RuleReader();
            HashSet<Download> downloads = new HashSet<Download>();
            foreach (JObject i in Version["libraries"])
            {
                rr.Execute(new Func<JArray>(() =>
                {
                    if (i.ContainsKey("rules"))
                        return (JArray)i["rules"];
                    return null;
                })(),
                () =>
                {
                    if (i.ContainsKey("natives"))
                    {
                        downloads.Add(new Download(
                            Source.native(i),
                            "libraries/" + (string)i["classifiers"]["natives-windows"]["path"],
                            (string)i["classifiers"]["natives-windows"]["hash"]
                            ));
                    }
                    else
                    {
                        downloads.Add(new Download(
                            Source.library(i),
                            "libraries/" + (string)i["artifact"]["path"],
                            (string)i["artifact"]["hash"]
                            ));
                    }
                });
            }
            checkFiles(downloads);
        }

        public override List<string> generateArgs()
        {
            throw new NotImplementedException();
        }

        public override List<string> generateVMArgs()
        {
            throw new NotImplementedException();
        }
    }
}
