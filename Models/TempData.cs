namespace SimpleBlog.Models
{
    public class TempData
    {
        private static Guid _accountId;
        private static string _nickName = string.Empty;

        public static Guid AccountId { get => _accountId; set => _accountId = value; }
        public static string AccountTableName { get => _nickName; set => _nickName = value; }
        public static string NickName { get => _nickName; set => _nickName = value; }
    }
}
