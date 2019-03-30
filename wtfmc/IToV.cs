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
        /// The profile. Decides where stuff
        /// are put.
        /// </summary>
        Profile Profile { get; set; }

        /// <summary>
        /// Find a version based on a version identifier.
        /// </summary>
        /// <param name="identifier">The identifier of a version</param>
        /// <returns></returns>
        IVersionParser GetVersion(string identifier);

        /// <summary>
        /// Find the latest version.
        /// </summary>
        /// <returns></returns>
        IVersionParser GetLatest();

        /// <summary>
        /// Find the latest snapshot.
        /// </summary>
        /// <returns></returns>
        IVersionParser GetLatestSnap();
    }
}
