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
    public interface IVersionParser {

        /// <summary>
        /// The version data.
        /// </summary>
        JObject Version { get; }
        
        // Check files referenced in the
        // version json. The lack of both the
        // file itself or the integrity would
        // cause it to be downloaded.
        void checkAssets();
        void checkLibraries();
        void checkClient();

        /// <summary>
        /// Generate the value for the -cp option.
        /// </summary>
        /// <returns>A comma-separated list of jars.</returns>
        string generateClasspath();

        /// <summary>
        /// Unpack natives.
        /// </summary>
        void unpackNatives();

        /// <summary>
        /// Generate command line arguments for game.
        /// </summary>
        List<string> generateArgs();

        /// <summary>
        /// Generate command line arguments for JVM.
        /// </summary>
        List<string> generateVMArgs();
    }
}
