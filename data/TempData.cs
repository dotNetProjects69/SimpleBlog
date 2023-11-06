namespace SimpleBlog.data
{
    public class TempData
    {
        private static Guid accountId;
        private static string accountTableName = string.Empty;

        public static Guid AccountId { get => accountId; set => accountId = value; }
        public static string AccountTableName { get => accountTableName; set => accountTableName = value; }
    }
}
