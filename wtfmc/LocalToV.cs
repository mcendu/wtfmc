using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc
{
    /// <summary>
    /// Inferred version manifest
    /// from directory structure.
    /// </summary>
    public sealed class LocalToV : IToV
    {
        public string GameDir { get; set; }

        public IVersion GetLatest(IDownloadSource dlsrc)
        {
            throw new NotImplementedException();
        }

        public IVersion GetLatestSnap(IDownloadSource dlsrc)
        {
            throw new NotImplementedException();
        }

        public IVersion GetVersion(string identifier, IDownloadSource dlsrc)
        {
            IEnumerable<string> dlist = Directory.EnumerateDirectories(GameDir);
            var q = from vd in dlist
                    where vd == identifier
                    select GameDir + vd;
            return
                MojangAPI.Version.Parse(
                    File.ReadAllText(
                        $"{q.First()}/{identifier}.json"
                    )
                );
        }
    }
}
