using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using wtfmc.Config;

namespace wtfmc
{
    /// <summary>
    /// Loads version data.
    /// </summary>
    public interface IVersion {

        /// <summary>
        /// The main class.
        /// </summary>
        string MainClass { get; }
        
        /// <summary>
        /// Check files referenced in the
        /// version json. 
        /// </summary>
        /// <param name="path">The game directory.</param>
        /// The lack of both the
        /// file itself or the integrity would
        /// cause it to be downloaded.
        void CheckData(string path);

        /// <summary>
        /// Generate the value for the -cp option.
        /// </summary>
        /// <returns>A comma-separated list of jars.</returns>
        string GenerateClasspath();

        /// <summary>
        /// Generate command line arguments for game.
        /// </summary>
        List<string> GenerateArgs(ILoginClient login, Profile profile);

        /// <summary>
        /// Generate command line arguments for JVM.
        /// </summary>
        List<string> GenerateVMArgs(ILoginClient login, Profile profile);
    }
}
