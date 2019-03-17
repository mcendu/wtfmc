using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    abstract class VersionCommon : IVersionParser
    {
        public abstract JObject Version { get; }

        public void checkFiles()
        {
            throw new NotImplementedException();
        }

        public string generateClasspath()
        {
            throw new NotImplementedException();
        }

        public abstract List<string> generateArgs();
        public abstract List<string> generateVMArgs();
    }
}
