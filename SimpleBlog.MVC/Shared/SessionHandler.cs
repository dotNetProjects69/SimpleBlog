using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.MVC.Shared
{
    public class SessionHandler : ISessionHandler
    {
        private readonly int _unAuthorizedId = -1;
        
        private HttpContext? Context => new HttpContextAccessor().HttpContext;
        private string AccountIdSessionKey => "accountId";

        public int GetOwnerId() => Context?.Session.GetInt32(AccountIdSessionKey) ?? -1;

        public void SetOwnerId(int id) => Context?.Session.SetInt32(AccountIdSessionKey, id);
        public void SetUnauthorizedId()
        {
            SetOwnerId(_unAuthorizedId);
        }
    }
}
