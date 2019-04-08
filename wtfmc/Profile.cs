﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// The type of profile.
        /// </summary>
        public ProfileType ProfileType { get; set; }

        /// <summary>
        /// The time a profile has been launched
        /// last time.
        /// </summary>
        public DateTime LastUsed { get; internal set; }

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
        public int? Width { get => width == null ? 854 : width ; set => width = value; }
        private int? width;

        /// <summary>
        /// Vertical resolution.
        /// </summary>
        public int? Height { get => height == null ? 480 : height; set => height = value; }
        private int? height;

        /// <summary>
        /// Path to JVM, or null for autodetection.
        /// </summary>
        public string JVM { get; set; }

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
                    return tov.GetLatest();
                case ProfileType.LatestSnapshot:
                    return tov.GetLatestSnap();
                default: // Always assume ProfileType.Custom upon null.
                    return tov.GetVersion(Version);
            }
        }

        /// <summary>
        /// Start the game.
        /// </summary>
        /// <param name="tov">The Table of Versions as a download source.</param>
        public void Launch(IToV tov, ILoginClient login)
        {
            IVersion version = Discover(tov);

            // ---------------------- //
            // Generate Command line. //
            // ---------------------- //
            Process proc = new Process();
            List<string> args = new List<string>();
            // JVM
            proc.StartInfo.FileName = JVM;
            args.Add(JVM);
            // JVM arguments
            args.AddRange(version.GenerateVMArgs());
            // Classpath
            args.Add("-cp");
            args.Add(version.GenerateClasspath());
            // Main class
            args.Add(version.MainClass);
            // Game arguments
            args.AddRange(version.GenerateArgs(login, this));

            // ---- //
            // Run. //
            // ---- //
            proc.Start();

        }
    }

    public enum ProfileType
    {
        Custom = 0,
        LatestRelease = 1,
        LatestSnapshot = 2
    }
}
