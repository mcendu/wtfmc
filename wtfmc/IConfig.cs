using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc
{
    /// <summary>
    /// The profile-independent
    /// configurations.
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// The underlying JSON data.
        /// </summary>
        JObject data { get; set; }

        /// <summary>
        /// The login data.
        /// </summary>
        ILoginClient Login { get; set; }

        /// <summary>
        /// Whether to enable snapshots.
        /// </summary>
        bool EnableSnapshots { get; set; }

        /// <summary>
        /// Whether to enable historical versions.
        /// </summary>
        /// <remarks>
        /// The term "historical versions" encom-
        /// pass the ages of Beta, Alpha, Infdev,
        /// Classic and Preclassic. A great deal
        /// of historicals, like the Tech test &
        /// all of Indev, are missing, and the
        /// majority of those included are modi-
        /// fied for some reason (not censorship).
        /// </remarks>
        bool EnableHistorical { get; set; }
    }
}
