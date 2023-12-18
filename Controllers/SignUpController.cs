using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models;
using SimpleBlog.Models.Authentication;
using System.Net;
using static System.Console;
using static SimpleBlog.Shared.GlobalParams;

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
            _signUpPagePath = "/Views/Authentication/SignUp.cshtml";

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
            using (SqliteConnection connection = new(GetAccountsDataPath()))
            {
                using var command = connection.CreateCommand();
                {
                    connection.Open();
                    SetNewGuid(model);
                    AddNewAccount(model, command);
                    AddAccountTable(model, command);
                }
            }
            return RedirectToAction("Index", "Posts");
        }

        private ErrorModel CheckAndSetError(SignUpModel model)
        {
            if (model.DetectBlankFields().StatusCode != HttpStatusCode.OK) 
                return model.DetectBlankFields();
            else if (model.CheckNickNameAlreadyUsed().StatusCode != HttpStatusCode.OK)
                return model.CheckNickNameAlreadyUsed();
            else if (model.CheckEmailAlreadyExist().StatusCode != HttpStatusCode.OK)
                return model.CheckEmailAlreadyExist();
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
                SetCurrentAccountTableNAme(model);
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
                SetCurrentAccointId(model);
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        private static void SetNewGuid(SignUpModel model)
        {
            model.Id = Guid.NewGuid();
        }

        private static void SetCurrentAccountTableNAme(SignUpModel model)
        {
            Models.TempData.AccountTableName = model.NickName;
        }

        private static void SetCurrentAccointId(SignUpModel model)
        {
            Models.TempData.AccountId = model.Id;
        }
    }
}
