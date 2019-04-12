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
            ProfileType = ProfileType.LatestRelease;
            DlSrc = new MojangAPI.DlSource();
            GameDir = $@"{Environment.GetEnvironmentVariable("APPDATA")}/.minecraft";
        }

        /// <summary>
        /// The type of profile.
        /// </summary>
        public ProfileType ProfileType { get; set; }

        /// <summary>
        /// The download source.
        /// </summary>
        public IDownloadSource DlSrc { get; set; }

        /// <summary>
        /// The time a profile has been launched
        /// last time.
        /// </summary>
        public string LastUsed { get; internal set; }

        /// <summary>
        /// The version to be launched.
        /// 
        /// If ProfileType is either LatestRelease
        /// or LatestSnapshot, this prop shall be
        /// ignored.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Horizontal resolution.
        /// </summary>
        public int? Width { get => width ?? 854; set => width = value; }
        private int? width;

        /// <summary>
        /// Vertical resolution.
        /// </summary>
        public int? Height { get => height ?? 480; set => height = value; }
        private int? height;

        /// <summary>
        /// Path to JVM, or null for autodetection.
        /// </summary>
        public string JVM { get => jvm == null ? Util.LocateJava() : jvm; set => jvm = value; }
        private string jvm;

        /// <summary>
        /// The JVM args used while launching.
        /// </summary>
        public string JVMArgs { get; set; }

        /// <summary>
        /// The place where game files are
        /// placed.
        /// </summary>
        public string GameDir { get; set; }

        /// <summary>
        /// Path to log configuration.
        /// </summary>
        public string LogConfig { get; set; }

        private IVersion Discover(IToV tov)
        {
            switch (ProfileType)
            {
                case ProfileType.LatestRelease:
                    return tov.GetLatest(DlSrc);
                case ProfileType.LatestSnapshot:
                    return tov.GetLatestSnap(DlSrc);
                default: // Always assume ProfileType.Custom upon null.
                    return tov.GetVersion(Version, DlSrc);
            }
        }

        /// <summary>
        /// Start the game.
        /// </summary>
        /// <param name="tov">The Table of Versions as a download source.</param>
        public void Launch(IToV tov, ILoginClient login)
        {
            string o = Util.SetCurrentDirectory(GameDir);
            // --------------- //
            // Check for data. //
            // --------------- //

            tov.GameDir = GameDir;
            IVersion version = Discover(tov);
            version.CheckData(GameDir);

            // ---------------------- //
            // Generate Command line. //
            // ---------------------- //

            Process proc = new Process();
            List<string> args = new List<string>();

            // JVM
            proc.StartInfo.FileName = JVM;

            // JVM arguments
            if (JVMArgs == null)
                args.AddRange(version.GenerateVMArgs(login, this));
            else
            {
                // Custom args
                args.Add(JVMArgs);
                // Classpath
                args.Add("-cp");
                args.Add(version.GenerateClasspath());
            }

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
