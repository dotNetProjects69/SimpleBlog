﻿namespace SimpleBlog.Models.Authentification
{
    public class SignInModel
    {
        private Guid _id;
        private string _nickName;
        private string _email;
        private string _password;
        private ErrorModel _error;

        public SignInModel()
        {
            _nickName = string.Empty;
            _email = string.Empty;
            _password = string.Empty;
            _error = new();
        }

        public string NickName { get => _nickName; set => _nickName = value; }
        public string Email { get => _email; set => _email = value; }
        public string Password { get => _password; set => _password = value; }
		public ErrorModel Error { get => _error; set => _error = value; }
        public Guid Id { get => _id; set => _id = value; }
    }
}
