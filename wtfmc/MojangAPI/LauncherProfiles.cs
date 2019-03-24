using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    /// <summary>
    /// launcher_profiles.json format, with wtfmc extensions.
    /// </summary>
    public class LauncherProfiles : IConfig
    {
        public LauncherProfiles()
        {
        }
        public LauncherProfiles(string path)
        {
            Data = JObject.Parse(File.ReadAllText(path));
        }
        private JObject data;
        public JObject Data {
            get => data;
            private set
            {
                data = value;
            }
        }
        private static void SetBool(JObject parent, string index, bool val)
        {
            if (val)
            {
                parent.Add(index, val);
            }
            else
            {
                if (parent.ContainsKey(index))
                {
                    parent.Remove(index);
                }
            }
        }

        private static bool GetBool(JObject parent, string index)
        {
            if (parent.ContainsKey(index))
            {
                return (bool)parent["index"];
            }
            else
                return false;
        }
        public ILoginClient Login
        {
            get
            {
                if (Data.ContainsKey("selectedUser"))
                {
                    return new MojangLogin
                    {
                        Data = Data
                    };
                }
                return null;
            }
            set
            {
                
            }
        }
        public bool EnableSnapshots
        {
            get => GetBool((JObject)Data["settings"], "enableSnapshots");
            set => SetBool((JObject)Data["settings"], "enableSnapshots", value);
        }
        public bool EnableHistorical {
            get => GetBool((JObject)Data["settings"], "enableHistorical");
            set => SetBool((JObject)Data["settings"], "enableHistorical", value);
        }
        public bool AdvancedOptions {
            get => GetBool((JObject)Data["settings"], "enableAdvanced");
            set => SetBool((JObject)Data["settings"], "enableAdvanced", value);
        }
        public List<IProfile> Profiles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
