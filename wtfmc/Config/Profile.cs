using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc.Config
{
    /// <summary>
    /// WTFMC profile format.
    /// </summary>
    public class Profile : JObject, IConfigStored
    {
        /// <summary>
        /// Construct a default, empty profile.
        /// </summary>
        public Profile() : base()
        {
            ProfileType = ProfileType.LatestRelease;
        }

        /// <summary>
        /// Construct a profile from a JSON.
        /// </summary>
        /// <param name="json"></param>
        public Profile(JObject json) : base(json)
        {

        }

        /// <summary>
        /// Set the key of a property.
        /// </summary>
        /// <param name="key"></param>
        internal void SetKey(string key, JToken value)
        {
            if (ContainsKey(key))
                this[key] = value;
            else
                Add(key, value);
        }

        /// <summary>
        /// The type of profile.
        /// </summary>
        public ProfileType ProfileType
        {
            get => (ProfileType)Enum.Parse(typeof(ProfileType), (string)this["type"]);
            set => this["type"] = value.ToString();
        }

        /// <summary>
        /// The time a profile has been launched
        /// last time.
        /// </summary>
        public string LastUsed
        {
            get => (string)this["lastUsed"];
            internal set => SetKey("lastUsed", value);
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
            get => (string)this["version"];
            set => SetKey("version", value);
        }

        /// <summary>
        /// Horizontal resolution.
        /// </summary>
        public int? Width
        {
            get => (int?)this["resolutionWidth"] ?? 854;
            set => SetKey("resolutionWidth", value);
        }

        /// <summary>
        /// Vertical resolution.
        /// </summary>
        public int? Height
        {
            get => (int?)this["resolutionHeight"] ?? 854;
            set => SetKey("resolutionHeight", value);
        }

        /// <summary>
        /// Path to JVM, or null for autodetection.
        /// </summary>
        public string JVM
        {
            get => (string)this["jvm"] ?? Util.LocateJava();
            set => SetKey("jvm", value);
        }

        /// <summary>
        /// The JVM args used while launching.
        /// </summary>
        public string JVMArgs
        {
            get => (string)this["jvmArgs"];
            set => SetKey("jvmArgs", value);
        }

        /// <summary>
        /// The place where game files are
        /// placed.
        /// </summary>
        public string GameDir
        {
            get => (string)this["gameDir"] ?? $@"{Environment.GetEnvironmentVariable("APPDATA")}/.minecraft";
            set => SetKey("gameDir", value);
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
            Util.GenDir(GameDir);
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

        public JObject ToJObject() => this;
    }

    public enum ProfileType
    {
        Custom = 0,
        LatestRelease = 1,
        LatestSnapshot = 2
    }
}
