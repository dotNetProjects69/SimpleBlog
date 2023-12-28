using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;
using SimpleBlog.Models.Authentication;
using System.Net;
using static SimpleBlog.Controllers.Extensions.AccountSql;
using static SimpleBlog.Models.TempData;

namespace SimpleBlog.Controllers.SignInControllers
{
    public abstract class SignInController : Controller
    {
        private readonly ILogger<SignInController> _logger;
        private readonly IConfiguration _configuration;
        private protected SignInModel _model;
        private protected readonly string _signInByNicknamePagePath;
        private protected readonly string _signInByEmailPagePath;

        protected SignInController(ILogger<SignInController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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
            string accountId = SelectFromTableByWhere("UserID", "NickName", _model.Nickname)[0];
            SetAccountIdToModel(accountId);
            HttpContext.Session.SetString(AccountIdSessionKey, _model.Id.ToString());
        }

        private void SetAccountIdToModel( string accountId)
        {
            _model.Id = new(accountId);
        }
    }
}
