using SimpleBlog.Models.Interfaces.AccountModelParts;

namespace SimpleBlog.Models.Account
{
    public class AccountInfoModel : IAccount, IEmail
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
    }
}
