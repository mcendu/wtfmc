using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Locate a JVM.
        /// Would prefer Java 8 over other Java revisions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method only searches the default installation
        /// directory, and would not check directories outside
        /// it.
        /// </para>
        /// <para>
        /// Aside from the "Java 8 first" rule, this method
        /// prefers later Java over older Java, and JDK over
        /// JRE, in descending order of precedence.
        /// </para>
        /// </remarks>
        /// <returns>The absolute path to the JVM.</returns>
        public static string locateJava()
        {
            string baseSPath;
            int selMajV = 0;
            int selSubV = 0;
            string selPath = null;
            if (Directory.Exists("C:\\Program Files\\Java"))
            {
                baseSPath = "C:\\Program Files\\Java";
            }
            else if (Directory.Exists("C:\\Program Files (x86)\\Java"))
            {
                baseSPath = "C:\\Program Files (x86)\\Java";
            }
            else
                return null;
            IEnumerable<string> sDirs = Directory.EnumerateFiles(baseSPath);
            foreach (string i in sDirs)
            {
                // Match Java 8
                Match mSubV = Regex.Match(i, @"(?=j(dk|re)1\.8\.0_)\d");
                if (mSubV.Success)
                {
                    short subV = short.Parse(mSubV.Value);
                    if (subV > selSubV)
                    {
                        selSubV = subV;
                        selPath = i;
                    }
                    continue;
                }

                // Match newer Java
                mSubV = Regex.Match(i, @"(?=j(dk|re)-(?<major>\d)\.\d\.)\d");
                if (mSubV.Success)
                {
                    short majV = short.Parse(mSubV.Groups["major"].Value);
                    short subV = short.Parse(mSubV.Value);
                    if (selMajV != 8 && majV > selMajV)
                    {
                        selMajV = majV;
                        selSubV = subV;
                        selPath = i;
                        continue;
                    }
                    else if (subV > selSubV)
                    {
                        selSubV = subV;
                        selPath = i;
                    }
                    continue;
                }

                // Match older Java
                mSubV = Regex.Match(i, @"(?=j(dk|re)(1\.(?<major>\d)\.\d_)\d");
                if (mSubV.Success)
                {
                    short majV = short.Parse(mSubV.Groups["major"].Value);
                    short subV = short.Parse(mSubV.Value);
                    if (majV > selMajV)
                    {
                        selMajV = majV;
                        selSubV = subV;
                        selPath = i;
                        continue;
                    }
                    else if (subV > selSubV)
                    {
                        selSubV = subV;
                        selPath = i;
                    }
                    continue;
                }
            }
            return selPath + @"\bin\javaw.exe";
        }
    }
}
