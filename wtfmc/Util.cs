using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc
{
    public static class Util
    {
        /// <summary>
        /// Convert a hexadecimal number to a little-endian byte array.
        /// </summary>
        /// <param name="input">A hexadecimal number.</param>
        /// <returns>A byte array.</returns>
        public static string bintohex(byte[] bin)
        {
            string o = "";
            foreach (byte b in bin)
            {
                o += b.ToString("X2");
            }
            return o.ToLower();
        }
    }
}
