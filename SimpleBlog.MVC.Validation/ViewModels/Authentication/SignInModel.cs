namespace SimpleBlog.MVC.Validation.ViewModels.Authentication
{
    public class SignInModel
    {
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public bool NicknameOrEmailEntered =>
            string.IsNullOrEmpty(Nickname.Trim()) || string.IsNullOrEmpty(Email.Trim());
    }
}
