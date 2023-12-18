using SimpleBlog.Models.Authentication;

namespace SimpleBlog.Models.Account
{
    public interface IAccount : INamed, ISurnamed
    {
        public Guid Id { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
    }
}
