using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Shared
{
    public class SessionHandler : ISessionHandler
    {
        private HttpContext? Context => new HttpContextAccessor().HttpContext;
        private string AccountIdSessionKey => "accountId";
        public string SessionOwnerId
        {
            get => Context?.Session.GetString(AccountIdSessionKey) ?? string.Empty;
            set => Context?.Session.SetString(AccountIdSessionKey, value);
        }
    }
}
