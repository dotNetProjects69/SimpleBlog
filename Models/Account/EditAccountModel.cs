using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleBlogTest")]

namespace SimpleBlog.Models.Account
{
    public class EditAccountModel : IAccount, IPassword, IEmail
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public IErrorModel Error { get; set; } = ErrorModel.Success;
        public Guid UserId { get; set; }
    }
}