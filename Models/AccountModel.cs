﻿namespace SimpleBlog.Models
{
    public class AccountModel
    {
        private Guid id;
        private string name = string.Empty;
        private string surname = string.Empty;
        private DateOnly dateOfBirth;
        private string email = string.Empty;
        private string password = string.Empty;
        private string nickName = string.Empty;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public DateOnly DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string NickName { get => nickName; set => nickName = value; }
    }
}
