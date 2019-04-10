using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc
{
    /// <summary>
    /// Table of versions (TOV).
    /// </summary>
    public interface IToV
    {
        /// <summary>
        /// Game Directory.
        /// </summary>
        string GameDir { get; set; }

        /// <summary>
        /// Find a version based on a version identifier.
        /// </summary>
        /// <param name="identifier">The identifier of a version</param>
        /// <returns></returns>
        IVersion GetVersion(string identifier, IDownloadSource dlsrc);

        /// <summary>
        /// Find the latest version.
        /// </summary>
        /// <returns></returns>
        IVersion GetLatest(IDownloadSource dlsrc);

        /// <summary>
        /// Find the latest snapshot.
        /// </summary>
        /// <returns></returns>
        IVersion GetLatestSnap(IDownloadSource dlsrc);
    }
}
