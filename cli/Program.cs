using System;
using wtfmc;
using wtfmc.Config;

namespace cli
{
    class Program
    {
        // Perform an offline launch.
        static void Main(string[] args)
        {
            Profile p = new Profile();
            ILoginClient login = new wtfmc.MojangAPI.Offline();
            login.Authenticate("Player", null);
            try
            {
                p.Launch(wtfmc.MojangAPI.ToV.Download(new wtfmc.MojangAPI.DlSource()), new wtfmc.MojangAPI.DlSource(), login);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
            Console.ReadKey();
        }
    }
}
