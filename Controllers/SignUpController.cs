using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Models;
using SimpleBlog.Models.Registration;
using System.Net;

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

        public IActionResult Index()
        {
            SignUpModel model = new();
			return View(_signUpPagePath, model);
		}

        public IActionResult CheckInputDataAndRegister(SignUpModel model)
        {
            while (true)
            {
				using (SqliteConnection connection = new (_configuration.GetConnectionString("AccountsData")))
				{
					connection.Open();

					using SqliteCommand command = new($"SELECT name " +
                                                      $"FROM sqlite_master " +
                                                      $"WHERE type='table' " +
                                                      $"AND name='{model.NickName}'", connection);
					object? result = command.ExecuteScalar();

					if (result != null)
					{
                        model.Error = new(HttpStatusCode.Conflict, "This nickname already used");
					}
				}
                return RegisterNewAccount(model);
            }
        }

        public IActionResult RegisterNewAccount(SignUpModel model)
        {
            using (SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData")))
            {
                using var command = connection.CreateCommand();
                connection.Open();
                model.Id = Guid.NewGuid();
                AddAccountTable(model, command);
                AddNewAccount(model, command);
            }
            return RedirectToAction("Index", "Posts");
        }

        private static void AddAccountTable(SignUpModel model, SqliteCommand command)
        {
            command.CommandText = $"create table [{model.NickName}] (" +
                                                  $"Id  integer primary key autoincrement, " +
                                                  $"Title text, Body text, " +
                                                  $"CreatedAt text, UpdatedAt text)";
            try
            {
                command.ExecuteNonQuery(); 
                Models.TempData.AccountTableName = model.NickName.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void AddNewAccount(SignUpModel model, SqliteCommand command)
        {
            command.CommandText = $"INSERT INTO AuthData (Name, Surname, DateOfBirth, Email, Password, UserID, NickName) " +
                                  $"VALUES ('{model.Name}', '{model.Surname}', " +
                                          $"'{model.DateOfBirth}', '{model.Email}', " +
                                          $"'{model.Password}', '{model.Id}', '{model.NickName}')";
            try
            {
                command.ExecuteNonQuery();
                Models.TempData.AccountId = model.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
