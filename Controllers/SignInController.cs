using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models.Registration;
using System.Net;
using SimpleBlog.Views.Registration;
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

        public IActionResult Index()
        {
            SignInModel model = new();
            return View(_signInPagePath, model);
        }

        public IActionResult LogIn(SignInModel signInModel)
        {
            signInModel ??= new();
            while (true)
            {
                signInModel.Error = CheckInputNickName(signInModel);
                if (signInModel.Error.StatusCode == HttpStatusCode.OK)
                {
                    Models.TempData.AccountTableName = signInModel.NickName;
                    return RedirectToAction("Index", "Posts");
                }
                return View(_signInPagePath, signInModel);
            }
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

            Console.WriteLine(result != null);
            if (result != null)
            {
                if (!CheckInputPassword(signInModel))
                    return new ErrorModel(HttpStatusCode.BadRequest, "Password not valid");
                else
                    return new ErrorModel();
            }
            else
                return new ErrorModel(HttpStatusCode.NotFound, "An account with such a nickname does not exist");
        }

        private bool CheckInputPassword(SignInModel signInModel)
        {
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            string password = signInModel.Password;
            string tableName = "AuthData";
            connection.Open();
            using SqliteCommand command = new($"Select Password from {tableName} where NickName = '{signInModel.NickName}'", connection);
            Console.WriteLine(command.CommandText);
            object? result = command.ExecuteScalar();
            if (result != null)
            {
                Console.WriteLine((string)result);
                if ((string)result == signInModel.Password)
                    return true;
            }
            return false;
        }
    }
}
