using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;

namespace SimpleBlog.Models.Authentication
{
    public class SignUpModel : IName, ISurname, IConfirmedPassword, IEmail, INickname
    {
        public Guid UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        // Browsers return blank fields as null
        public string? Surname { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public IErrorModel Error { get; set; } = ErrorModel.Success;
        public string ConfirmedPassword { get; set; } = string.Empty;

        public void Debug()
        {
            Console.WriteLine($"Id - {UserId}\n" +
                              $"Name - {Name}\n" +
                              $"Surname - {Surname}\n" +
                              $"Date of birth - {DateOfBirth}\n" +
                              $"Email - {Email}\n" +
                              $"Password - {Password}\n" +
                              $"Nickname - {Nickname}\n");
        }
    }
}