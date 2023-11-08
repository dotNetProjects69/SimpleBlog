using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Models;
using SimpleBlog.Models.Registration;
using System.Net;
using System.Net.Mail;

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

        public IActionResult Index(SignUpModel? model = null)
        {
            model ??= new();
			return View(_signUpPagePath, model);
		}

        public IActionResult CheckInputDataAndRegister(SignUpModel model)
        {
            while (true)
            {
                if (string.IsNullOrWhiteSpace(model.Name) || !IsValidEmail(model.Email) ||
                    string.IsNullOrWhiteSpace(model.NickName) || string.IsNullOrWhiteSpace(model.Password))
                {
                    model.Error = new(HttpStatusCode.BadRequest, "Some required fields are blank");
                    return Index(model);
                }

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
                        return Index(model);
                    }
                }
                
                return Register(model);
            }
        }

        public IActionResult Register(SignUpModel model)
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

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

    }
}
