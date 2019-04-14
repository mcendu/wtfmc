using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace wtfmc
{
    internal static partial class Util
    {
        public static ILoginClient ParseLogin(JObject json)
        {
            LoginType authtype = (LoginType)Enum.Parse(typeof(LoginType), (string)json["authtype"]);
            switch (authtype)
            {
                case LoginType.Mojang:
                    return new MojangAPI.MojangLogin(json);
                case LoginType.Offline:
                    return new MojangAPI.Offline(json);
            }
            return null;
        }
    }
}
