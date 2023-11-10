using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models.Authentification;
using System.Net;
using Microsoft.Data.Sqlite;
using SimpleBlog.Models;

namespace SimpleBlog.Controllers
{
    public class SignInController : Controller
    {
        private readonly ILogger<SignInController> _logger;
        private readonly IConfiguration _configuration;
        string _signInPagePath;

        public SignInController(ILogger<SignInController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _signInPagePath = "/Views/Authentification/SignIn.cshtml";
        }

        public IActionResult Index(SignInModel? model = null)
        {
            model ??= new();
            return View(_signInPagePath, model);
        }

        public IActionResult LogIn(SignInModel model)
        {
            model ??= new();
            model.Error = CheckInputNickName(model);
            if (model.Error.StatusCode == HttpStatusCode.OK)
            {
                Models.TempData.AccountTableName = model.NickName;
                return RedirectToAction("Index", "Posts");
            }
            return Index(model);
        }

        private ErrorModel CheckInputNickName(SignInModel signInModel)
        {
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            string tableName = signInModel.NickName;
            connection.Open();
            using SqliteCommand command = new($"SELECT name " +
                                              $"FROM sqlite_master " +
                                              $"WHERE type='table' AND name='{tableName}'", connection);
            object? result = command.ExecuteScalar();

            if (result == null)
                return new ErrorModel(HttpStatusCode.NotFound, "An account with such a nickname does not exist");
            return CheckInputPassword(signInModel);
        }

        private ErrorModel CheckInputPassword(SignInModel model)
        {
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            string password = model.Password;
            string tableName = "AuthData";
            connection.Open();
            using SqliteCommand command = new($"Select Password from {tableName} where NickName = '{model.NickName}'", connection);
            object? result = command.ExecuteScalar();
            if (result == null || (string)result != model.Password)
                return new ErrorModel(HttpStatusCode.BadRequest, "Password not valid");
            return new ErrorModel();
        }

        
    }
}
