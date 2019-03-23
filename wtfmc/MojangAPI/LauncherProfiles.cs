using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    public class LauncherProfiles : IConfig
    {
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
        public ILoginClient Login { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableSnapshots { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableHistorical { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<IProfile> Profiles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
