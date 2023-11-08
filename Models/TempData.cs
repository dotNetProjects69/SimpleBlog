namespace SimpleBlog.Models
{
    public class TempData
    {
        private static Guid accountId;
        private static string nickName = string.Empty;

        public static Guid AccountId { get => accountId; set => accountId = value; }
        public static string AccountTableName { get => nickName; set => nickName = value; }
        public static string NickName { get => nickName; set => nickName = value; }
    }
}
