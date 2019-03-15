using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc
{
    public static class Util
    {
        /// <summary>
        /// Convert a byte array to its hex representation.
        /// </summary>
        /// <param name="bin">The byte array.</param>
        /// <returns>The hex representation of bin.</returns>
        public static string bintohex(byte[] bin)
        {
            string o = "";
            foreach (byte b in bin)
            {
                o += b.ToString("X2");
            }
            return o.ToLower();
        }

        public static bool checkIntegrity(Stream hInput, string cksum)
            => bintohex(SHA1.Create().ComputeHash(hInput)) == cksum;
    }
}
