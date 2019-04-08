using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace wtfmc
{
    /// <summary>
    /// WTFMC configuration format.
    /// </summary>
    public class WTFConfig
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public WTFConfig()
        {
            Profiles = new Dictionary<string, Profile>();
            Users = new List<ILoginClient>();
            SelectedUser = 0;
        }

        /// <summary>
        /// From a file with JSON data.
        /// </summary>
        /// <param name="filepath">Path to file.</param>
        public static WTFConfig FromJSON(string filepath)
            => new JsonSerializer().Deserialize<WTFConfig>
            (new JsonTextReader
                (new StreamReader(filepath))
            );

        /// <summary>
        /// The current launcher version.
        /// </summary>
        public int LauncherVersion => 0;

        /// <summary>
        /// The version of official launcher
        /// it is meant to be compatible with.
        /// </summary>
        public int MojangLauncher => 21;

        /// <summary>
        /// A list of profiles by ID.
        /// </summary>
        public IDictionary<string, Profile> Profiles { get; }

        /// <summary>
        /// A list of accounts.
        /// </summary>
        public IEnumerable<ILoginClient> Users { get; }

        /// <summary>
        /// The selected user entry.
        /// </summary>
        public int SelectedUser { get; set; }

        /// <summary>
        /// Convert profile to launcher_profiles.json format.
        /// Only Mojang logins are preserved in the output.
        /// </summary>
        public string ToLPF()
        {
            throw new NotImplementedException();
        }
    }
}
