using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models;
using SimpleBlog.Models.Account;
using SimpleBlog.Models.Authentification;
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

        public IActionResult Index(SignUpModel? model = null)
        {
            model ??= new();
			return View(_signUpPagePath, model);
		}

        public IActionResult Register(SignUpModel model)
        {
            model ??= new ();
            model.Error = CheckAndSetError(model);
            model.Surname ??= string.Empty;
            if (model.Error.StatusCode != HttpStatusCode.OK)
                return Index(model);
            using (SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData")))
            {
                using var command = connection.CreateCommand();
                {
                    connection.Open();
                    model.Id = Guid.NewGuid();
                    AddNewAccount(model, command);
                    AddAccountTable(model, command);
                }
            }
            return RedirectToAction("Index", "Posts");
        }

        private ErrorModel CheckAndSetError(SignUpModel model)
        {
            if (model.CheckBlankFields().StatusCode != HttpStatusCode.OK) 
                return model.CheckBlankFields();
            else if (model.CheckNickName<AccountInfoModel>().StatusCode != HttpStatusCode.OK)
                return model.CheckNickName<AccountInfoModel>();
            else if (model.CheckEmailAlreadyExist<AccountInfoModel>().StatusCode != HttpStatusCode.OK)
                return model.CheckEmailAlreadyExist<AccountInfoModel>();
            return new ErrorModel();
        }

        private void AddAccountTable(SignUpModel model, SqliteCommand command)
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

        private void AddNewAccount(SignUpModel model, SqliteCommand command)
        {
            command.CommandText = $"INSERT INTO AuthData (Name, Surname, DateOfBirth, Email, Password, UserID, NickName) " +
                                  $"VALUES ('{model.Name.Trim()}', '{model.Surname.Trim()}', " +
                                          $"'{model.DateOfBirth}', '{model.Email.Trim()}', " +
                                          $"'{model.Password.Trim()}', '{model.Id}', '{model.NickName.Trim()}')";
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
