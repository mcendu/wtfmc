using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    public abstract class Launcher : ILauncher
    {
        public abstract ILoginClient login { get; }
        public abstract string VMEXE { get; set; }


        public string[] genArgs(string gameDir, string version, string[] customOpts, string[] customVMOpts)
        {
            if (!Directory.Exists(gameDir))
                throw new DirectoryNotFoundException();
            if (VMEXE == null)
            {
                VMEXE = Util.locateJava() ?? throw new FileNotFoundException();
            }

            // Parse version string.
            Match m = Regex.Match(version, @"(?<maj>\d)\.(?<min>)\.(?<pat>).*");
            string maj = m.Groups["maj"].Value;
            string min = m.Groups["min"].Value;
            string pat = m.Groups["pat"].Value;

            // cd into game directory.
            Directory.SetCurrentDirectory(gameDir);

            List<string> vmArgs = customVMOpts == null ? new List<string>() : customVMOpts.ToList();
            List<string> args = customOpts == null ? new List<string>() : customOpts.ToList();

            // Check for specific types of custom options.
            bool customXms = false;
            bool customXmx = false;

            foreach (string i in customVMOpts)
            {
                if (Regex.IsMatch(i, @"^-Xms"))
                {
                    customXms = true;
                }
                if (Regex.IsMatch(i, @"^-Xmx"))
                {
                    customXmx = true;
                }
            }

            // Load version.json.
            JToken versiondata = JToken.Parse(File.ReadAllText($"versions/{version}/{version}.json"));

            // Set .minecraft.
            args.Add("--gameDir");
            args.Add(Directory.GetCurrentDirectory());
            // Set username.
            args.Add("--username");
            args.Add(login.Username);
            // Set uuid.
            args.Add("--uuid");
            args.Add(login.ID);
            // Set assets info.
            args.Add("--assetIndex");
            args.Add((string)versiondata["assetIndex"]["id"]);
            // Set login type.
            args.Add(login.LoginType);

            // Generate classpath.
            VersionLoader v;
        }
        public abstract void Launch(string[] args);
    }
}
