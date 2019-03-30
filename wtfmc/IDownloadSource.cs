using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc
{
    /// <summary>
    /// Converts a URL to mojang to the
    /// one provided by a download source.
    /// </summary>
    public interface IDownloadSource
    {
        /// <summary>
        /// Convert a URL to Mojang to one
        /// provided by a download source.
        /// </summary>
        /// <param name="source">The Mojang URL.</param>
        /// <returns></returns>
        string Translate(string source);
    }
}
