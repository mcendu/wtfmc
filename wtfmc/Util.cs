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
    internal static partial class Util
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

        // A shell on Dir.SetCD
        public static string SetCurrentDirectory(string path)
        {
            string ret = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(path);
            return ret;
        }

        public static void GenDir(string destination)
        {
            string d = "";
            List<string> compo = Path.GetFullPath(destination).Split(new char[] { Path.DirectorySeparatorChar }).ToList();
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
                GenDir(Path.GetDirectoryName(i.Path));
                if (!(new Func<bool>(() =>
                {
                    try
                    {
                        bool b;
                        using (FileStream f = File.Open(i.Path, FileMode.Open))
                        {
                            b = CheckIntegrity(f, i.Hash);
                        }
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
        public static void RmR(string path)
        {
            foreach (string i in Directory.EnumerateDirectories(path))
                RmR(Path.Combine(path, i));
            foreach (string i in Directory.EnumerateFiles(path))
                File.Delete(Path.Combine(path, i));
            Directory.Delete(path);
        }
    }
}
