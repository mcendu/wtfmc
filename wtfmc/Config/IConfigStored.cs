using System;
using System.Collections.Generic;
using System.Text;

namespace wtfmc.Config
{
    /// <summary>
    /// Represents an object that can be stored in an WTFMC Config file.
    /// </summary>
    public interface IConfigStored
    {
        /// <summary>
        /// Convert object to JSON.
        /// </summary>
        /// <returns>The JSON representation of the object.</returns>
        Newtonsoft.Json.Linq.JObject ToJObject();
    }
}
