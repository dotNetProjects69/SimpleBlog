using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;

namespace SimpleBlog.Models.Authentication
{
    public class SignInModel : IPassword, IEmail, INickname
    {
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public IErrorModel Error { get; set; } = ErrorModel.Success;
        public Guid UserId { get; set; }
    }
}