using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc.MojangAPI
{
    public class DlSource : IDownloadSource
    {
        public string Translate(string source) => source;
    }
}
