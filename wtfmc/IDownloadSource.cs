using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc
{
    /// <summary>
    /// A source from which files are downloaded.
    /// </summary>
    interface IDownloadSource
    {
        /// <summary>
        /// Gives the path to version_manifest.json.
        /// </summary>
        Uri getVersionManifest();

        /// <summary>
        /// Gives the path to a client's json.
        /// </summary>
        /// <param name="version">A version of Minecraft.</param>
        Uri getJson(string version);

        /// <summary>
        /// Gives the path to a client.jar.
        /// </summary>
        /// <param name="json">The accompanying json.</param>
        Uri getClient(JObject json);

        /// <summary>
        /// Gives the path to a server.jar.
        /// </summary>
        /// <param name="json">The accompanying json.</param>
        Uri getServer(JObject json);

        /// <summary>
        /// Gives the path to an assets index.
        /// </summary>
        /// <param name="json">The json of a version.</param>
        /// <returns></returns>
        Uri getAssetsIndex(JObject json);

        /// <summary>
        /// Gives a list of path to required libs.
        /// </summary>
        /// <param name="json">The json of a version.</param>
        /// <returns></returns>
        List<Uri> getLibraries(JObject json);

        /// <summary>
        /// Gives the path to a logger config file.
        /// </summary>
        /// <param name="json">The json of a version.</param>
        /// <returns></returns>
        Uri getLoggerConf(JObject json);
    }
}
