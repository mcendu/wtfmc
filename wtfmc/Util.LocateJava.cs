using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace wtfmc
{
    internal static partial class Util
    {
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
        public static string LocateJava()
        {
            // look for base directory.
            string baseSPath;
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

            // Linq discover Java versions.
            IEnumerable<Tuple<short, short, string>> jvmlist = GetJavaVersion(Directory.EnumerateDirectories(baseSPath));
            // Find Java 8s.
            var j8s = from jvm in jvmlist
                      where jvm.Item1 == 8
                      orderby jvm.Item2 descending
                      select jvm;
            if (j8s.Count() > 0)
            {
                selPath = j8s.First().Item3;
            }
            else
            {
                j8s = from jvm in jvmlist
                      orderby jvm.Item2 descending
                      select jvm;

                if (j8s.Count() > 0)
                {
                    selPath = j8s.First().Item3;
                }
                else
                {
                    return null;
                }
            }
#if DEBUG
            return selPath + @"\bin\java.exe";
#else
            return selPath + @"\bin\javaw.exe";
#endif
        }

        static IEnumerable<Tuple<short, short, string>> GetJavaVersion(IEnumerable<string> names)
        {
            foreach (string name in names)
            {
                // Match older Java
                Match mSubV = Regex.Match(name, @"j(dk|re)1\.(?<major>\d)\.\d_(?<minor>\d)");
                if (mSubV.Success)
                {
                    short majV = short.Parse(mSubV.Groups["major"].Value);
                    short subV = short.Parse(mSubV.Groups["minor"].Value);
                    yield return new Tuple<short, short, string>(majV, subV, name);
                }

                // Match newer Java
                mSubV = Regex.Match(name, @"j(dk|re)-(?<major>\d)\.\d\.(?<minor>\d)");
                if (mSubV.Success)
                {
                    short majV = short.Parse(mSubV.Groups["major"].Value);
                    short subV = short.Parse(mSubV.Groups["minor"].Value);
                    yield return new Tuple<short, short, string>(majV, subV, name);
                }
            }
        }
    }
}
