using System.Xml.Linq;

namespace SimpleBlog.Models.Account
{
    public interface IAccountModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
    }
}
