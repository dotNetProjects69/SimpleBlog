using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Models;

namespace SimpleBlog.Controllers
{
    public class SignUpController : Controller
    {

        private readonly ILogger<SignUpController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _signUpPagePath;

        public SignUpController(ILogger<SignUpController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _signUpPagePath = "/Views/Authentification/SignUp.cshtml";

        }

        public IActionResult ShowSignUpPage()
        {
            AccountModel model = new();
            return View(_signUpPagePath, model);
        }

        public IActionResult RegisterNewAccount(AccountModel account)
        {
            // In future we must go to posts for this account

            using (SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData")))
            {
                using var command = connection.CreateCommand();
                connection.Open();
                account.Id = Guid.NewGuid();
                AddAccountTable(account, command);
                AddNewAccount(account, command);
            }
            return RedirectToAction("Index", "Posts");
        }

        private static void AddAccountTable(AccountModel account, SqliteCommand command)
        {
            command.CommandText = $"create table [{account.NickName}] (" +
                                                  $"Id  integer primary key autoincrement, " +
                                                  $"Title text, Body text, " +
                                                  $"CreatedAt text, UpdatedAt text)";
            try
            {
                command.ExecuteNonQuery(); 
                data.TempData.AccountTableName = account.NickName.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void AddNewAccount(AccountModel account, SqliteCommand command)
        {
            command.CommandText = $"INSERT INTO AuthData (Name, Surname, DateOfBirth, Email, Password, UserID, NickName) " +
                                  $"VALUES ('{account.Name}', '{account.Surname}', " +
                                          $"'{account.DateOfBirth}', '{account.Email}', " +
                                          $"'{account.Password}', '{account.Id}', '{account.NickName}')";
            try
            {
                command.ExecuteNonQuery();
                data.TempData.AccountId = account.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
