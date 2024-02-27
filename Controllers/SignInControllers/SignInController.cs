using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models.Authentication;
using SimpleBlog.Shared;
using System.Net;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;

namespace SimpleBlog.Controllers.SignInControllers
{
    public abstract class SignInController : Controller
    {
        private readonly ILogger<SignInController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHandler _sessionHandler;
        private protected SignInModel _model;
        private protected readonly string _signInByNicknamePagePath;
        private protected readonly string _signInByEmailPagePath;

        protected SignInController(ILogger<SignInController> logger,
                                   IConfiguration configuration,
                                   ISessionHandler? sessionHandler = null)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHandler = sessionHandler ?? new SessionHandler();
            _signInByNicknamePagePath = "/Views/Authentication/SignInByNickname.cshtml";
            _signInByEmailPagePath = "/Views/Authentication/SignInByEmail.cshtml";
        }

        public abstract IActionResult Index(SignInModel? model = null);

        private protected bool StatusCodeIsOk(SignInModel model)
        {
            return model.Error.StatusCode == HttpStatusCode.OK;
        }

        private protected void SetCurrentAccountIdToGlobal()
        {
            string accountId = 
                SelectFromTableByWhere(
                        "UserID", 
                        "NickName", 
                        _model.Nickname)
                    .ElementAtOrDefault(0) ?? string.Empty;
            SetAccountIdToModel(accountId);
            _sessionHandler.SessionOwnerId = _model.UserId.ToString();
        }

        private void SetAccountIdToModel( string accountId)
        {
            _model.UserId = new(accountId);
        }
    }
}
