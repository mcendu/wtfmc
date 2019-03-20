using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc
{
    /// <summary>
    /// Assets Index reader.
    /// </summary>
    interface IAssetsIndex
    {
        /// <summary>
        /// The assets index.
        /// </summary>
        JObject Assets { get; }

        void checkAssets();
    }
}
