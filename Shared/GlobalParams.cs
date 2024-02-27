using System.Diagnostics;

namespace SimpleBlog.Shared
{
    public class GlobalParams
    {
        private static readonly IConfiguration Configuration = 
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public static string GetAccountsDataPath()
        {
            bool isDebug = false;

#if DEBUG
            isDebug = true;
#endif

            if (isDebug)
                return Configuration.GetConnectionString("AccountsDataDebug") ?? "";
            
            return Configuration.GetConnectionString("AccountsData") ?? "";
        }

        public static string GetAbsoluteDbDirectoryPath()
        {
            bool isDebug = false;
            string result = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
#if DEBUG
            isDebug = true;
#endif
            if (isDebug)
                return Path.Combine(result, "Debug");

            return Path.Combine(result, "Release");
        }

        public static string GetRelativeDbDirectoryPath()
        {
            bool isDebug = false;
#if DEBUG
            isDebug = true;
#endif
            return Path.Combine("data", isDebug ? "Debug" : "Release");
        }
    }
}
