using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models;
using SimpleBlog.Models.Authentification;
using System.Net;

namespace SimpleBlog.Controllers
{
    public class SignInController : Controller
    {
        private readonly ILogger<SignInController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _signInPagePath;
        private readonly string _accountsData;

        public SignInController(ILogger<SignInController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _signInPagePath = "/Views/Authentification/SignIn.cshtml";
            _accountsData = _configuration.GetConnectionString("AccountsData") ?? "";
        }

        public IActionResult Index(SignInModel? model = null)
        {
            model ??= new();
            return View(_signInPagePath, model);
        }

        public IActionResult LogInByNickname(SignInModel model)
        {
            model ??= new();
            model.Error = CheckNickName(model);
            if (model.Error.StatusCode == HttpStatusCode.OK)
                model.Error = CheckInputPassword(model);
            if (model.Error.StatusCode == HttpStatusCode.OK)
            {
                Models.TempData.AccountTableName = model.NickName;
                Models.TempData.AccountId = new Guid(GetGuidByNickname(model));
                return RedirectToAction("Index", "Posts");
            }
            return Index(model);
        }

        public IActionResult LogInByEmail(SignInModel model)
        {
            model ??= new();
            model.Error = CheckInputEmail(model);
            if (model.Error.StatusCode == HttpStatusCode.OK)
            {
                Models.TempData.AccountTableName = model.NickName;
                Models.TempData.AccountId = new Guid(GetGuidByEmail(model));
                return RedirectToAction("Index", "Posts");
            }
            return Index(model);
        }

        private ErrorModel CheckInputEmail(SignInModel model)
        {
            throw new NotImplementedException();
        }

        private string GetGuidByNickname(SignInModel model)
        {
            return GetGuid("Nickname", model.NickName);
        }

        private string GetGuidByEmail(SignInModel model)
        {
            return GetGuid("Email", model.Email);
        }

        private string GetGuid(string filterParam, string fiterValue)
        {
            return AccountSql.SelectFromTable("UserID", filterParam, fiterValue).ElementAt(0);
        }

        private ErrorModel CheckNickName(SignInModel signInModel)
        {
            return signInModel.CheckNickNameDoesNotExist();
        }

        private ErrorModel CheckInputPassword(SignInModel model)
        {
            string password = model.Password;
            string tableName = "AuthData";
            IEnumerable<string> result = AccountSql.SelectFromTable("Password", "NickName", model.NickName, tableName);
            if (result.Count() != 0 && result.ElementAt(0) != password)
                return new ErrorModel(HttpStatusCode.BadRequest, "Password not valid");
            return new ErrorModel();
        }

        
    }
}
