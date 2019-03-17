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
            if (Login.LoginType != "mojang")
                return false;
            JObject data = JObject.Parse(Login.Data);
            return !(bool)data["selectedProfile"]["paid"];
        }

        public ILoginClient Login { get; set; }

        public JObject ProfileOpts { get; set; }

        public bool Execute(JObject rule)
        {
            if (rule.ContainsKey("os"))
            {
                if (!((string)rule["os"]["name"] == getOS() || (string)rule["os"]["name"] == "unknown"))
                    return false;
                if (!((string)rule["os"]["arch"] == getArch()))
                    return false;
            }
            if (rule.ContainsKey("features"))
            {
                if (!isDemo())
                    return false;
            }
            return true;
        }
    }
}
