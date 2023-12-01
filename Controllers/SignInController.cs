using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models;
using SimpleBlog.Models.Authentification;
using System.Net;

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
            _signInByNicknamePagePath = "/Views/Authentification/SignInByNickname.cshtml";
            _signInByEmailPagePath = "/Views/Authentification/SignInByEmail.cshtml";
        }

        public abstract IActionResult Index(SignInModel? model = null);

        private protected string GetGuid(string filterParam, string fiterValue)
        {
            return AccountSql.SelectFromTable("UserID", filterParam, fiterValue).ElementAt(0);
        }

        private protected bool StatusCodeIsOK(SignInModel model)
        {
            return model.Error.StatusCode == HttpStatusCode.OK;
        }

        private protected virtual void SetAccountTableName(SignInModel model)
        {
            Models.TempData.AccountTableName = model.NickName;
        }

        private protected void ValidateInputPassword(SignInModel model)
        {
            ErrorModel errorModel = new ();
            string password = model.Password;
            string tableName = "AuthData";
            IEnumerable<string> result = AccountSql.SelectFromTable("Password", "NickName", model.NickName, tableName);
            if (result.Any() && result.ElementAt(0) != password)
            {
                errorModel.StatusCode = HttpStatusCode.BadRequest;
                errorModel.Message = "Password not valid";
            }
            model.Error = errorModel;
        }

        
    }
}
