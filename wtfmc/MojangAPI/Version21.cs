using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    /// <summary>
    /// Minecraft Java JSON format from 1.13.
    /// Changes from the previous version (1.9) include:
    /// * Minimal launcher version changed to 21 (from 18).
    /// * Arguments spec moved from minecraftArguments to arguments.game.
    /// * Added another arg spec for JVM.
    /// * Explicit specification of --demo and --width/--height.
    /// * A new convention for libs with natives
    /// ** One entry without natives, one with natives.
    /// </summary>
    public sealed class Version21 : Version
    {
        public Version21(JObject vdata) : base(vdata)
        {
        }

        public override string GenerateClasspath()
        {
            RuleReader rr = new RuleReader();
            string classpath = "";
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
                    if (!i.ContainsKey("natives"))
                        classpath += Path.GetFullPath("libraries/" + (string)i["artifact"]["path"]) + ";";
                });
            }
            classpath += $"versions/{VID}/{VID}.jar";
            return classpath;
        }

        public override void CheckLibraries(string path)
        {
            string o = SetCurrentDirectory(path);
            RuleReader rr = new RuleReader();
            HashSet<Download> downloads = new HashSet<Download>();
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
                    if (i.ContainsKey("natives"))
                    {
                        downloads.Add(new Download(
                            (string)i["classifiers"]["natives-windows"]["url"],
                            "libraries/" + (string)i["classifiers"]["natives-windows"]["path"],
                            (string)i["classifiers"]["natives-windows"]["hash"]
                            ));
                    }
                    else
                    {
                        downloads.Add(new Download(
                            (string)i["artifact"]["url"],
                            "libraries/" + (string)i["artifact"]["path"],
                            (string)i["artifact"]["hash"]
                            ));
                    }
                });
            }
            Util.CheckFiles(downloads);
            Directory.SetCurrentDirectory(o);
        }

        public override List<string> GenerateArgs(ILoginClient login, Profile profile)
        {
            List<string> arguments = new List<string>();
            RuleReader rr = new RuleReader
            {
                Login = login
            };
            Hashtable arghash = GenParamHash(profile);
            // Apply default parameters.
            JArray input = (JArray)vdata["arguments"]["game"];
            foreach (JToken i in input)
            {
                if (i.HasValues)
                {
                    rr.Execute((JArray)(i["rules"]), () =>
                    {
                        arguments.AddRange(from string s in i["value"]
                                           select string.Format(new Formatter(), s, arghash));
                    });
                }
                else
                {
                    // Boilerplate code found here...
                    arguments.Add(string.Format(new Formatter(), (string)i, arghash));
                }
            }
            return arguments;
        }

        // Copy paste code from above.
        public override List<string> GenerateVMArgs()
        {
            List<string> arguments = new List<string>();
            RuleReader rr = new RuleReader();
            Hashtable arghash = new Hashtable();
            arguments.Add(GenXmx());
            // Apply default parameters.
            JArray input = (JArray)vdata["arguments"]["game"];
            foreach (JToken i in input)
            {
                if (i.HasValues)
                {
                    rr.Execute((JArray)(i["rules"]), () =>
                    {
                        arguments.AddRange(from string s in i["value"]
                                           select string.Format(new Formatter(), s, arghash));
                    });
                }
                else
                {
                    // Boilerplate code found here...
                    arguments.Add(string.Format(new Formatter(), (string)i, arghash));
                }
            }
            return arguments;
        }
    }
}
