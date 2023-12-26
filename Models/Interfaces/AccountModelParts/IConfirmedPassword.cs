namespace SimpleBlog.Models.Interfaces.AccountModelParts
{
    public interface IConfirmedPassword : IPassword
    {
        public string ConfirmedPassword { get; set; }
    }
}
