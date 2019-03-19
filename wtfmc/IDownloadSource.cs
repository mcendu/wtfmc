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
    public interface IDownloadSource
    {
        /// <summary>
        /// Gives the path to version_manifest.json.
        /// </summary>
        Uri versionManifest();

        /// <summary>
        /// Gives the path to a version info.
        /// </summary>
        /// <param name="index">The position in versions array.</param>
        /// <returns></returns>
        Uri json(int index);

        /// <summary>
        /// Gives the path to a version info.
        /// </summary>
        /// <param name="version">The version id.</param>
        /// <returns></returns>
        Uri json(string version);

        /// <summary>
        /// Gives the path to a client.jar.
        /// </summary>
        /// <param name="json">The accompanying json.</param>
        Uri client(JObject json);

        /// <summary>
        /// Gives the path to a server.jar.
        /// </summary>
        /// <param name="json">The accompanying json.</param>
        Uri server(JObject json);

        /// <summary>
        /// Gives the path to an assets index.
        /// </summary>
        /// <param name="json">The json of a version.</param>
        /// <returns></returns>
        Uri assetsIndex(JObject json);

        /// <summary>
        /// Generate the path to a library.
        /// </summary>
        /// <param name="json">The json of a version.</param>
        /// <returns></returns>
        Uri library(JObject libDesc);

        /// <summary>
        /// Generate the path to a library native.
        /// </summary>
        /// <param name="json">The json of a version.</param>
        /// <returns></returns>
        Uri native(JObject libDesc);

        /// <summary>
        /// Gives the path to a logger config file.
        /// </summary>
        /// <param name="json">The json of a version.</param>
        /// <returns></returns>
        Uri loggerConf(JObject json);
    }
}
