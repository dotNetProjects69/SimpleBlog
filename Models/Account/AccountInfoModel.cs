namespace SimpleBlog.Models.Account
{
    public class AccountInfoModel : IAccount
    {
        private Guid _id;
        private string name = string.Empty;
        private string surname = string.Empty;
        private DateOnly dateOfBirth;   
        private string email = string.Empty;
        private string nickName = string.Empty;

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public DateOnly DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string Email { get => email; set => email = value; }
        public string NickName { get => nickName; set => nickName = value; }
        public Guid Id { get => _id; set => _id = value; }
    }
}
