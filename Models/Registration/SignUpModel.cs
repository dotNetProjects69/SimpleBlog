namespace SimpleBlog.Models.Registration
{
    public class SignUpModel
    {
        private int id;
        private string name = string.Empty;
        private string surname = string.Empty;
        private DateOnly dateOfBirth;
        private string email = string.Empty;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public DateOnly DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string Email { get => email; set => email = value; }
    }
}
