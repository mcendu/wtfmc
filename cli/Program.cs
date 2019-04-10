using System;
using wtfmc;

namespace cli
{
    class Program
    {
        // Perform an offline launch.
        static void Main(string[] args)
        {
            Profile p = new Profile();
            ILoginClient login = new wtfmc.MojangAPI.Offline();
            login.Authenticate(null, null);
            p.Launch(wtfmc.MojangAPI.ToV.Download(new wtfmc.MojangAPI.DlSource()), login);
        }
    }
}
