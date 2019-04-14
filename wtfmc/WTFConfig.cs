using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public WTFConfig(JObject json)
        {
            Profiles = (IDictionary<string, Profile>)from JProperty p in (JObject)json["profiles"]
                       select new KeyValuePair<string, Profile>(p.Name, (Profile)p.Value);
            Users = (IList<ILoginClient>)from JObject o in (JArray)json["auth"]
                                         select Util.ParseLogin(o);
        }

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
        public IList<ILoginClient> Users { get; }

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
