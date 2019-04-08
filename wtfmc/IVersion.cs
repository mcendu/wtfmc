using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc
{
    /// <summary>
    /// Loads version data.
    /// </summary>
    public interface IVersion {

        /// <summary>
        /// Login data.
        /// </summary>
        ILoginClient Login { get; set; }

        /// <summary>
        /// The main class.
        /// </summary>
        string MainClass { get; }
        
        // Check files referenced in the
        // version json. The lack of both the
        // file itself or the integrity would
        // cause it to be downloaded.
        void CheckAssetsIndex(string path);
        void CheckLibraries(string path);
        void CheckClient(string path);

        /// <summary>
        /// Generate the value for the -cp option.
        /// </summary>
        /// <returns>A comma-separated list of jars.</returns>
        string GenerateClasspath();

        /// <summary>
        /// Unpack natives.
        /// </summary>
        void UnpackNatives();

        /// <summary>
        /// Generate command line arguments for game.
        /// </summary>
        List<string> GenerateArgs(ILoginClient login, Profile profile);

        /// <summary>
        /// Generate command line arguments for JVM.
        /// </summary>
        List<string> GenerateVMArgs();
    }
}
