using SimpleBlog.Models.Interfaces.AccountModelParts;

namespace SimpleBlog.Models.Authentication
{
    public class SignInModel : IPassword, IEmail, INickname
    {
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ErrorModel Error { get; set; } = new();
        public Guid Id { get; set; }

        public void Debug()
        {
            Console.WriteLine($"Id - {Id}\n" +
                              $"Email - {Email}\n" +
                              $"Password - {Password}\n" +
                              $"Nickname - {Nickname}\n");
        }
    }
}
