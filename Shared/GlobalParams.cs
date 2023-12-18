namespace SimpleBlog.Shared
{
    public class GlobalParams
    {
        private static readonly IConfiguration Configuration = new ConfigurationBuilder()
                                                               .AddJsonFile("appsettings.json")
                                                               .Build();

        public static string GetAccountsDataPath()
        {
            bool isDebug = false;

#if DEBUG
            isDebug = true;
#endif

            if (isDebug)
                return Configuration.GetConnectionString("AccountsDataDebug") ?? "";
            else
                return Configuration.GetConnectionString("AccountsData") ?? "";
        }
    }
}
