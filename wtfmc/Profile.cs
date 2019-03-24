using System;
using System.Collections.Generic;
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
        /// The type of profile; can be either
        /// "latest-release", "latest-snapshot"
        /// or "custom".
        /// </summary>
        public string ProfileType { get; set; }

        /// <summary>
        /// The time a profile has been launched
        /// last time.
        /// </summary>
        public DateTime LastUsed { get; internal set; }

        /// <summary>
        /// The version to be launched. No use
        /// if not custom.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Horizontal resolution.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Vertical resolution.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Path to JVM, or null for autodetection.
        /// </summary>
        public string JVM { get; set; }

        /// <summary>
        /// The JVM args used while launching.
        /// </summary>
        public string JVMArgs { get; set; }

        /// <summary>
        /// The place where game files are actually
        /// placed.
        /// </summary>
        public string GameDir { get; set; }

        /// <summary>
        /// Path to log configuration.
        /// </summary>
        public string LogConfig { get; set; }
    }
}
