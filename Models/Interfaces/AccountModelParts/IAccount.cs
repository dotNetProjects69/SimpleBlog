namespace SimpleBlog.Models.Interfaces.AccountModelParts
{
    public interface IAccount : IName, ISurname, INickname
    {
        public Guid Id { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
    }
}
