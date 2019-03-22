using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace wtfmc
{
    /// <summary>
    /// A profile with profile-specific configs,
    /// for example JVM arguments, resolution etc.
    /// </summary>
    /// <remarks>
    /// Setting an argument to null means the usage
    /// of a default value.
    /// </remarks>
    public interface IProfile
    {
        /// <summary>
        /// The configuration data.
        /// </summary>
        JObject Config { get; set; }

        /// <summary>
        /// The identifier of the profile.
        /// </summary>
        string ProfileID { get; }

        /// <summary>
        /// The version of Minecraft to launch.
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// The custom width.
        /// </summary>
        int? Width { get; set; }

        /// <summary>
        /// The custom height=.
        /// </summary>
        int? Height { get; set; }

        /// <summary>
        /// The path to the JVM, or null
        /// for autodetection.
        /// </summary>
        string JVMPath { get; set; }

        /// <summary>
        /// Extra JVM arguments, apart from what
        /// is defined in the json.
        /// </summary>
        IEnumerable<string> JVMArgs { get; set; }

        /// <summary>
        /// Whether to launch the game fullscreen.
        /// </summary>
        bool Fullscreen { get; set; }

        /// <summary>
        /// The directory to store the actual game
        /// data for the profile.
        /// </summary>
        string Directory { get; set; }

        string LogConfig { get; set; }
    }
}
