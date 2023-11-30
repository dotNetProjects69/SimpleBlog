namespace SimpleBlog.Models.Authentification
{
    public interface IVerifiableCore
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }

        public void Debug();
    }
}
