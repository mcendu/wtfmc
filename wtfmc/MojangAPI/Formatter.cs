using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace wtfmc.MojangAPI
{
    public class Formatter
    {
        public string Format(string format, Hashtable table)
        {
            Match match;
            Regex regex = new Regex(@"\$\{(.+)\}");
            while (regex.IsMatch(format))
            {
                match = regex.Match(format);
                string key = match.Groups[1].Value;
                format = regex.Replace(format, table[key].ToString());
            }
            return format;
        }
    }
}
