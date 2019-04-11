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
                        classpath += Path.GetFullPath("libraries/" + (string)i["downloads"]["artifact"]["path"]) + ";";
                });
            }
            classpath += $"versions/{VID}/{VID}.jar";
            return classpath;
        }

        protected override void CheckLibraries(string path)
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
                            (string)i["downloads"]["classifiers"]["natives-windows"]["url"],
                            "libraries/" + (string)i["downloads"]["classifiers"]["natives-windows"]["path"],
                            (string)i["downloads"]["classifiers"]["natives-windows"]["sha1"]
                            ));
                    }
                    else
                    {
                        downloads.Add(new Download(
                            (string)i["downloads"]["artifact"]["url"],
                            "libraries/" + (string)i["downloads"]["artifact"]["path"],
                            (string)i["downloads"]["artifact"]["sha1"]
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
            Hashtable arghash = GenParamHash(login, profile);
            // Apply default parameters.
            JArray input = (JArray)vdata["arguments"]["game"];
            foreach (JToken i in input)
            {
                if (i.HasValues)
                {
                    rr.Execute((JArray)(i["rules"]), () =>
                    {
                        if (i["value"].HasValues)
                        {
                            foreach (string v in i["value"]) {
                                arguments.Add(new Formatter().Format(v, arghash));
                            }
                        } else
                        {
                            arguments.Add(new Formatter().Format((string)i["value"], arghash));
                        }
                    });
                }
                else
                {
                    // Boilerplate code found here...
                    arguments.Add(new Formatter().Format((string)i, arghash));
                }
            }
            return arguments;
        }

        // Copy paste code from above.
        public override List<string> GenerateVMArgs(ILoginClient login, Profile profile)
        {
            List<string> arguments = new List<string>();
            RuleReader rr = new RuleReader();
            Hashtable arghash = GenParamHash(login, profile);
            // Add heap limit.
            arguments.Add(GenXmx());
            // Apply default parameters.
            JArray input = (JArray)vdata["arguments"]["jvm"];
            foreach (JToken i in input)
            {
                if (i.HasValues)
                {
                    rr.Execute((JArray)(i["rules"]), () =>
                    {
                        if (i["value"].HasValues)
                        {
                            foreach (string v in i["value"])
                            {
                                arguments.Add(new Formatter().Format(v, arghash));
                            }
                        }
                        else
                        {
                            arguments.Add(new Formatter().Format((string)i["value"], arghash));
                        }
                    });
                }
                else
                {
                    // Boilerplate code found here...
                    arguments.Add(new Formatter().Format((string)i, arghash));
                }
            }
            return arguments;
        }
    }
}
