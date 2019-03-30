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
        public Profile Profile { get; set; }

        public IVersionParser GetLatest()
        {
            throw new NotImplementedException();
        }

        public IVersionParser GetLatestSnap()
        {
            throw new NotImplementedException();
        }

        public IVersionParser GetVersion(string identifier)
        {
            IEnumerable<string> dlist = Directory.EnumerateDirectories(Profile.GameDir);
            var q = from vd in dlist
                    where vd == identifier
                    select Profile.GameDir + vd;
            return
                MojangAPI.Version.Parse(
                    File.ReadAllText(
                        $"{q.First()}/{identifier}.json"
                    )
                );
        }
    }
}
