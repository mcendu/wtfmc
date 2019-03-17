using System.IO;

namespace wtfmc
{
    /// <summary>
    /// Launches Minecraft.
    /// </summary>
    interface ILauncher
    {
        /// <summary>
        /// Auth client.
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
        /// <param name="customOpts">List of custom options.</param>
        /// <param name="customVMOpts">List of custom JVM options.</param>
        /// <returns>
        /// A <code>string[]</code> consisting of command line args.
        /// The 0th entry of the returned value is the name of the
        /// exe file (in wtfmc.MojangAPI.Launch, java).
        /// </returns>
        string[] genArgs(string gameDir, string version, string[] customOpts, string[] customVMOpts);

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
