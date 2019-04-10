using System;
using Newtonsoft.Json.Linq;

namespace wtfmc
{
    /// <summary>
    /// Reads rules in a rule JSON object,
    /// and determine if it is true.
    /// </summary>
    class RuleReader
    {
        private static string getArch() => "x86";

        private static string getOS()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return "windows";
                case PlatformID.MacOSX:
                    return "osx";
                case PlatformID.Unix:
                    return "linux"; // Almost every other Unix is Linux
                default:
                    return "unknown";
            }
        }

        private static string getVersion()
        {
            return Environment.OSVersion.VersionString;
        }

        private bool isDemo()
        {
            if (Login.LoginType != LoginType.Mojang)
                return false;
            JObject data = Login.Data;
            return !(bool)data["selectedProfile"]["paid"];
        }

        private bool HasCustomResolution()
        {
            if (Width == null || Height == null)
                return false;
            return true;
        }

        public ILoginClient Login { get; set; }

        public int? Width { get; set; }
        public int? Height { get; set; }

        private bool Determine(JObject rule)
        {
            if (rule.ContainsKey("os"))
            {
                // TODO: Add OS name regex matching to
                // enable Windows-10 specific optimizations.
                JObject os = (JObject)rule["os"];
                if (os.ContainsKey("name"))
                    if ((string)os["name"] != "unknown" && (string)os["name"] != getOS())
                        return false;
                if (os.ContainsKey("arch"))
                    if ((string)os["arch"] != getArch())
                        return false;
            }
            if (rule.ContainsKey("features"))
            {
                JObject feats = (JObject)rule["features"];
                if (feats.ContainsKey("is_demo_user"))
                {
                    if ((bool)feats["is_demo_user"] != isDemo())
                        return false;
                    if ((bool)feats["has_custom_resolution"] != HasCustomResolution())
                        return false;
                }
            }
            return true;
        }

        public void Execute(JArray rule, Action action)
        {
            if (rule == null)
            {
                action();
                return;
            }
            foreach (JObject i in rule)
            {
                if (Determine(i))
                {
                    if ((string)i["action"] == "allow")
                        continue;
                    else // "disallow", or invalid action
                        return;
                } else
                {
                    if ((string)i["action"] == "allow")
                        return;
                    else // "disallow", or invalid action
                        continue;
                }
            }
            action();
        }
    }
}
