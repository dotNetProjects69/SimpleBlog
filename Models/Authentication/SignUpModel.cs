using SimpleBlog.Models.Interfaces.AccountModelParts;

namespace SimpleBlog.Models.Authentication
{
    public class SignUpModel : IName, ISurname, IConfirmedPassword, IEmail, INickname
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public ErrorModel Error { get; set; } = new();
        public string ConfirmedPassword { get; set; } = string.Empty;

        public void Debug()
        {
            Console.WriteLine($"Id - {Id}\n" +
                              $"Name - {Name}\n" +
                              $"Surname - {Surname}\n" +
                              $"Date of birth - {DateOfBirth}\n" +
                              $"Email - {Email}\n" +
                              $"Password - {Password}\n" +
                              $"Nickname - {Nickname}\n");
        }

    }
}
