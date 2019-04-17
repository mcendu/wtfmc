using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace wtfmc.Config
{
    /// <summary>
    /// WTFMC configuration format.
    /// </summary>
    public class ConfigRoot : JObject, IConfigStored
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConfigRoot() : base()
        {

        }

        public ConfigRoot(JObject json) : base(json)
        {

        }

        /// <summary>
        /// A list of profiles by ID.
        /// </summary>
        public IDictionary<string, Profile> Profiles
            => this["profiles"].ToDictionary(
                i => ((JProperty)i).Name,
                i => (Profile)((JProperty)i).Value);

        /// <summary>
        /// The currently logged in user.
        /// </summary>
        public ILoginClient User => Util.ParseLogin((JObject)this["user"]);

        /*
         * TODO: Support multiple logins.
         * The interface for multiple logins
         * is specified below.
         *
        
        /// <summary>
        /// A list of accounts.
        /// </summary>
        public IList<ILoginClient> Users { get; }

        /// <summary>
        /// The selected user entry.
        /// </summary>
        public int SelectedUser { get; set; }

         *
         * End of interface specification
         */

        /// <summary>
        /// Convert profile to launcher_profiles.json format.
        /// Only Mojang logins are preserved in the output.
        /// </summary>
        public string ToLPF()
        {
            throw new NotImplementedException();
        }

        public JObject ToJObject() => this;
    }
}
