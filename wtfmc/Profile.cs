using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc
{
    /// <summary>
    /// WTFMC profile format.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Construct a default, empty profile.
        /// </summary>
        public Profile()
        {
            data = new JObject();
            ProfileType = ProfileType.LatestRelease;
        }

        /// <summary>
        /// Construct a profile from a JSON.
        /// </summary>
        /// <param name="json"></param>
        public Profile(JObject json)
        {
            data = json;
            Width = (int)json["width"];
            Height = (int)json["height"];
            JVM = (string)json["jvmPath"];
            JVMArgs = (string)json["jvmArgs"];
            GameDir = (string)json["gameDir"];
        }

        public readonly JObject data;

        /// <summary>
        /// Returns the underlying JSON structure.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => data.ToString();

        public static explicit operator Profile(JToken json)
        {
            return new Profile((JObject)json);
        }

        /// <summary>
        /// The type of profile.
        /// </summary>
        public ProfileType ProfileType
        {
            get => (ProfileType)Enum.Parse(typeof(ProfileType), (string)data["type"]);
            set => data["type"] = value.ToString();
        }

        /// <summary>
        /// The time a profile has been launched
        /// last time.
        /// </summary>
        public string LastUsed
        {
            get => (string)data["lastUsed"];
            internal set
            {
                if (data.ContainsKey("lastUsed"))
                    data["lastUsed"] = value;
                else
                    data.Add("lastUsed", value);
            }
        }

        /// <summary>
        /// The version to be launched.
        /// 
        /// If ProfileType is either LatestRelease
        /// or LatestSnapshot, this prop shall be
        /// ignored.
        /// </summary>
        public string Version
        {
            get => (string)data["version"];
            set => data["version"] = value;
        }

        /// <summary>
        /// Horizontal resolution.
        /// </summary>
        public int? Width
        {
            get => (int?)data["resolutionWidth"] ?? 854;
            set
            {
                if (data.ContainsKey("resolutionWidth"))
                    data["resolutionWidth"] = value;
                else
                    data.Add("resolutionWidth", value);
            }
        }

        /// <summary>
        /// Vertical resolution.
        /// </summary>
        public int? Height
        {
            get => (int?)data["resolutionHeight"] ?? 854;
            set
            {
                if (data.ContainsKey("resolutionHeight"))
                    data["resolutionHeight"] = value;
                else
                    data.Add("resolutionHeight", value);
            }
        }

        /// <summary>
        /// Path to JVM, or null for autodetection.
        /// </summary>
        public string JVM
        {
            get => (string)data["jvm"] ?? Util.LocateJava();
            set
            {
                if (data.ContainsKey("jvm"))
                    data["jvm"] = value;
                else
                    data.Add("jvm", value);
            }
        }

        /// <summary>
        /// The JVM args used while launching.
        /// </summary>
        public string JVMArgs
        {
            get => (string)data["jvmArgs"];
            set
            {
                if (data.ContainsKey("jvmArgs"))
                    data["jvmArgs"] = value;
                else
                    data.Add("jvmArgs", value);
            }
        }

        /// <summary>
        /// The place where game files are
        /// placed.
        /// </summary>
        public string GameDir
        {
            get => (string)data["gameDir"] ?? $@"{Environment.GetEnvironmentVariable("APPDATA")}/.minecraft";
            set
            {
                if (data.ContainsKey("gameDir"))
                    data["gameDir"] = value;
                else
                    data.Add("gameDir", value);
            }
        }

        /// <summary>
        /// Path to log configuration.
        /// </summary>
        public string LogConfig { get; set; }

        private IVersion Discover(IToV tov, IDownloadSource dlSrc)
        {
            switch (ProfileType)
            {
                case ProfileType.LatestRelease:
                    return tov.GetLatest(dlSrc);
                case ProfileType.LatestSnapshot:
                    return tov.GetLatestSnap(dlSrc);
                default: // Always assume ProfileType.Custom upon null.
                    return tov.GetVersion(Version, dlSrc);
            }
        }

        /// <summary>
        /// Start the game.
        /// </summary>
        /// <param name="tov">The Table of Versions as a download source.</param>
        public void Launch(IToV tov, IDownloadSource dlSrc, ILoginClient login)
        {
            string o = Util.SetCurrentDirectory(GameDir);
            // --------------- //
            // Check for data. //
            // --------------- //

            tov.GameDir = GameDir;
            IVersion version = Discover(tov, dlSrc);
            version.CheckData(GameDir);

            // ---------------------- //
            // Generate Command line. //
            // ---------------------- //

            Process proc = new Process();
            List<string> args = new List<string>();

            // JVM
            proc.StartInfo.FileName = JVM;

            // JVM arguments
            if (JVMArgs != null)
            {
                // Custom args
                args.Add(JVMArgs);
                // Classpath
                args.Add("-cp");
                args.Add(version.GenerateClasspath());
            }
            else
                args.AddRange(version.GenerateVMArgs(login, this));

            // Main class
            args.Add(version.MainClass);

            // Game arguments
            args.AddRange(version.GenerateArgs(login, this));

            // ---- //
            // Run. //
            // ---- //

            // Update timestamp.
            LastUsed = DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo.UniversalSortableDateTimePattern);
            // Set arguments.
            foreach (string i in args)
                proc.StartInfo.Arguments += '"' + i + '"' + " ";
            // Start process
            proc.Start();

            System.IO.Directory.SetCurrentDirectory(o);
        }
    }

    public enum ProfileType
    {
        Custom = 0,
        LatestRelease = 1,
        LatestSnapshot = 2
    }
}
