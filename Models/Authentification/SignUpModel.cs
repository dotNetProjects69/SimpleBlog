﻿namespace SimpleBlog.Models.Authentification
{
    public class SignUpModel : IVerifiableFull
    {
        private Guid _id;
        private string _name = string.Empty;
        private string _surname = string.Empty;
        private DateOnly _dateOfBirth;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private ErrorModel _error = new();
        private string _nickName = string.Empty;

        public Guid Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Surname { get => _surname; set => _surname = value; }
        public DateOnly DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }
        public string Email { get => _email; set => _email = value; }
        public string Password { get => _password; set => _password = value; }
		public string NickName { get => _nickName; set => _nickName = value; }
        public ErrorModel Error { get => _error; set => _error = value; }

        public void Debug()
        {
            Console.WriteLine($"Id - {_id}\n" +
                              $"Name - {_name}\n" +
                              $"Surname - {_surname}\n" +
                              $"Date of birth - {DateOfBirth}\n" +
                              $"Email - {_email}\n" +
                              $"Password - {_password}\n" +
                              $"Nickname - {_nickName}\n");
        }
    }
}
