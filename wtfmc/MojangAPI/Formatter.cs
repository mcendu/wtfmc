using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace wtfmc.MojangAPI
{
    public class Formatter : IFormatProvider, ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!(arg is Hashtable))
                throw new FormatException();
            Hashtable table = arg as Hashtable;
            Match match;
            Regex regex = new Regex(@"\$\{(.+)\}");
            while (regex.IsMatch(format))
            {
                match = regex.Match(format);
                string key = match.Groups[1].Value;
                format = regex.Replace(format, table[key] as string);
            }
            return format;
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(IFormatProvider))
                return this;
            else
                return null;
        }
    }
}
