using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    /// <summary>
    /// Parts of the version.json that
    /// are mosly consistent across
    /// versions.
    /// </summary>
    abstract class VersionCommon : IVersionParser
    {
        public abstract JObject Version { get; }

        public void checkLibraries()
        {
            throw new NotImplementedException();
        }

        public void checkClient()
        {
            throw new NotImplementedException();
        }

        public string generateClasspath()
        {
            throw new NotImplementedException();
        }

        public void genLibraryDirectory()
        {
            throw new NotImplementedException();
        }

        public void unpackNatives()
        {
            throw new NotImplementedException();
        }

        public abstract void checkAssets();
        public abstract List<string> generateArgs();
        public abstract List<string> generateVMArgs();
    }
}
