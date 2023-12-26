using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;
using SimpleBlog.Models.Authentication;
using System.Net;
using static SimpleBlog.Controllers.Extensions.AccountSql;
using static SimpleBlog.Models.TempData;

namespace SimpleBlog.Controllers
{
    public abstract class SignInController : Controller
    {
        private readonly ILogger<SignInController> _logger;
        private readonly IConfiguration _configuration;
        private protected readonly string _signInByNicknamePagePath;
        private protected readonly string _signInByEmailPagePath;

        public SignInController(ILogger<SignInController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration; 
            _signInByNicknamePagePath = "/Views/Authentication/SignInByNickname.cshtml";
            _signInByEmailPagePath = "/Views/Authentication/SignInByEmail.cshtml";
        }

        public abstract IActionResult Index(SignInModel? model = null);

        private protected string GetGuid(string filterParam, string filterValue)
        {
            return SelectFromTableByWhere("UserID", filterParam, filterValue).First();
        }

        private protected bool StatusCodeIsOk(SignInModel model)
        {
            return model.Error.StatusCode == HttpStatusCode.OK;
        }

        private protected virtual void SetCurrentNickname(SignInModel model)
        {
            HttpContext.Session.SetString(NicknameSessionKey, model.Nickname);
        }

        
    }
}
