using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace wtfmc.MojangAPI
{
    /// <summary>
    /// Offline mode login.
    /// </summary>
    class Offline : ILoginClient
    {
        public Offline()
        {

        }

        public string Username { get; private set; }

        public string Data { get => Username; set => Username = value; }

        public string AccessToken => null;

        public string ID { get; private set; }

        public string LoginType => "offline";

        /// <summary>
        /// Dump username to Data.
        /// </summary>
        /// <param name="email">Username.</param>
        /// <param name="passwd">Never used, should always be null.</param>
        public void Authenticate(string email, string passwd)
        {
            byte[] id = new byte[16];
            Username = email;
            new Random().NextBytes(id);
            ID = Util.bintohex(id);
        }

        public bool CheckAvailable()
        {
            return true;
        }

        public void LogOut()
        {
        }

        public void Refresh()
        {
        }
    }
}
