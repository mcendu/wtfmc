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
    internal static class Util
    {
        /// <summary>
        /// Convert a byte array to its hex representation.
        /// </summary>
        /// <param name="bin">The byte array.</param>
        /// <returns>The hex representation of bin.</returns>
        public static string Bintohex(byte[] bin)
        {
            string o = "";
            foreach (byte b in bin)
            {
                o += b.ToString("X2");
            }
            return o.ToLower();
        }

        /// <summary>
        /// Check the integrity of a file.
        /// </summary>
        /// <param name="hInput"></param>
        /// <param name="cksum"></param>
        /// <returns>True if the sums match, false otherwise.</returns>
        public static bool CheckIntegrity(Stream hInput, string cksum)
            => Bintohex(SHA1.Create().ComputeHash(hInput)) == cksum;

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

        // A shell on Dir.SetCD
        public static string SetCurrentDirectory(string path)
        {
            string ret = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(path);
            return ret;
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

        public static void GenDir(string destination)
        {
            string d = "";
            List<string> compo = Path.GetFullPath(destination).Split(new char[] { Path.DirectorySeparatorChar }).ToList();
            compo.RemoveAt(compo.Count - 1);
            foreach (string j in compo)
            {
                d = Path.Combine(d, j);
                if (!Directory.Exists(d))
                {
                    Directory.CreateDirectory(d);
                }
            }
        }

        public static void CheckFiles(IEnumerable<Download> filedata)
        {
            foreach (Download i in filedata)
            {
                GenDir(i.Path);
                if (!(new Func<bool>(() =>
                {
                    try
                    {
                        FileStream f = File.Open(i.Path, FileMode.Open);
                        bool b = CheckIntegrity(f, i.Hash);
                        f.Dispose();
                        Console.WriteLine($"Checking {i.Path} ({i.Hash})");
                        return b;
                    }
                    catch (IOException)
                    {
                        return false;
                    }
                })()))
                {
                    Console.WriteLine($"Downloading {i.Path}");
                    Downloader.DownloadAsync(i).Wait();
                }
            }
        }

        /// <summary>
        /// Temp directory.
        /// </summary>
        public static string TEMP => Environment.OSVersion.Platform == PlatformID.Win32NT
            ? Environment.GetEnvironmentVariable("TEMP")
            : "/tmp";

        /// <summary>
        /// Remove a directory along with its contents.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Named after "$ rm -r", from Unix.
        /// </para>
        /// <para>
        /// This method only works for directories;
        /// Using it on files can cause an exception
        /// to be thrown. For that purpose, use
        /// <code>System.IO.File.Delete</code>.
        /// </para>
        /// </remarks>
        /// <param name="path">Path of the directory.</param>
        public static void RmR (string path)
        {
            foreach (string i in Directory.EnumerateDirectories(path))
                RmR(Path.Combine(path, i));
            foreach (string i in Directory.EnumerateFiles(path))
                File.Delete(Path.Combine(path, i));
            Directory.Delete(path);
        }
    }
}
