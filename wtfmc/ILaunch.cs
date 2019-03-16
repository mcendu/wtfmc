using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc
{
    /// <summary>
    /// Launches Minecraft.
    /// </summary>
    interface ILaunch
    {
        /// <summary>
        /// Auth client. This field is null in offline mode.
        /// </summary>
        ILoginClient login { get; }

        /// <summary>
        /// The JVM executable.
        /// </summary>
        string VMEXE { get; set; }

        /// <summary>
        /// Generate command line arguments.
        /// </summary>
        /// <param name="gameDir">Directory of the game.</param>
        /// <param name="version">The version name.</param>
        /// <returns>
        /// A <code>string[]</code> consisting of command line args.
        /// The 0th entry of the returned value is the name of the
        /// exe file (in wtfmc.MojangAPI.Launch, java).
        /// </returns>
        string[] genArgs(string gameDir, string version);

        /// <summary>
        /// Launch Minecraft.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="DriveNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        /// <exception cref="IOException"></exception>
        void Launch(string[] args);
    }
}
