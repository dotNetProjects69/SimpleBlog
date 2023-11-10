namespace SimpleBlog.Models.Authentification
{
    public interface IVerifiableData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NickName { get; set ; }

        public void Debug();
    }
}
