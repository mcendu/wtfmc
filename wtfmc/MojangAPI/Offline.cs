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
        public Offline()
        {
            Data = new JObject
            {
                { "authtype", LoginType.ToString() }
            };
        }

        public Offline(JObject data) => Data = data;

        public LoginType LoginType => wtfmc.LoginType.Offline;

        public string AccessToken => "00000000000000000000000000000000";

        public string Username => (string)Data["username"];

        public string ID => (string)Data["id"];

        public bool LoggedIn => Data.ContainsKey("username");

        public JObject Data { get; set; }

        public void Authenticate(string email, string passwd)
        {
            if (email == null)
                throw new BadAuthException("Username is empty.");
            byte[] i = new byte[16];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
                rng.GetBytes(i);
            Data = new JObject
            {
                { "authtype", LoginType.ToString() },
                { "username", email },
                { "id", Util.Bintohex(i) }
            };
        }

        public void LogOut()
        {
        }

        public void Refresh()
        {
        }

        public JObject ToJObject() => Data;
    }
}
