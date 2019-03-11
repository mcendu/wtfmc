using System;
using System.Net.Http;

namespace wtfmc
{
    public class MojangLoginData : LoginData
    {
        string clientToken;
        string profileID;
        string profileName;
    }
    public class MojangLogin : ILoginService
    {
        public MojangLoginData Authenticate(string email, string passwd)
        {
            throw new NotImplementedException();
        }

        public bool CheckAvailable()
        {
            throw new NotImplementedException();
        }

        public void LogOut(MojangLoginData access)
        {
            throw new NotImplementedException();
        }

        public MojangLoginData Refresh(MojangLoginData access)
        {
            throw new NotImplementedException();
        }
    }
}
