using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wtfmc.MojangAPI
{
    public sealed class Offline : ILoginClient
    {
        public LoginType LoginType => wtfmc.LoginType.Offline;

        public string AccessToken => "00000000000000000000000000000000";

        public string Username => (string)Data["username"];

        public string ID => (string)Data["id"];

        public JObject Data { get; set; }
        public JObject AdditionalData { get => null; set { } }

        public void Authenticate(string email, string passwd)
        {
            byte[] i = new byte[16];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(i);
            Data = new JObject
            {
                { "authtype", LoginType.ToString() },
                { "username", email },
                { "id", Util.bintohex(i) }
            };
        }

        public void LogOut()
        {
        }

        public void Refresh()
        {
        }
    }
}
