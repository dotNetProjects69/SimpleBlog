using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;
using SimpleBlog.Models.Registration;
using System.Net;
using SimpleBlog.Views.Registration;
using Microsoft.Data.Sqlite;

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

        public IActionResult ShowSignUpPage()
        {
            SignInModel model = new();
            return View(_signInPagePath, model);
        }

        public IActionResult LogIn(SignInModel signInModel)
        {
            signInModel ??= new();
            while (true)
            {
                signInModel.StatusCode = CheckInputNickName(signInModel);
                if (signInModel.StatusCode == HttpStatusCode.OK)
                {
                    data.TempData.AccountTableName = signInModel.NickName;
                    return RedirectToAction("Index", "Posts");
                }
                return View(_signInPagePath, signInModel);
            }
        }

        private HttpStatusCode CheckInputNickName(SignInModel signInModel)
        {
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            string tableName = signInModel.NickName;
            connection.Open();
            using SqliteCommand command = new($"SELECT name " +
                                              $"FROM sqlite_master " +
                                              $"WHERE type='table' AND name='{tableName}'", connection);
            object? result = command.ExecuteScalar();

            if (result != null)
            {
                if(CheckInputPassword(signInModel))
                    return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
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
