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

        /// <summary>
        /// The download source.
        /// </summary>
        IDownloadSource Source { get; set; }

        /// <summary>
        /// Login data.
        /// </summary>
        ILoginClient Login { get; set; }
        
        // Check files referenced in the
        // version json. The lack of both the
        // file itself or the integrity would
        // cause it to be downloaded.
        void checkAssetsIndex();
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
