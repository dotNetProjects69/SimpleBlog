using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models;
using SimpleBlog.Models.Account;
using SimpleBlog.Models.Authentification;
using System.Net;
using System.Reflection;

namespace SimpleBlog.Controllers
{
    public class EditAccountController : Controller
    {
        private readonly ILogger<EditAccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _editAccountPagePath;

        public EditAccountController(ILogger<EditAccountController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _editAccountPagePath = "/Views/PersonalAccount/EditAccount.cshtml";
        }

        public IActionResult Index(EditAccountModel model)
        {
            model ??= new();
            return View(_editAccountPagePath, model);
        }

        public IActionResult Update(EditAccountModel model)
        {
            model ??= new();
            model.Error = CheckAndSetError(model);
            model.Surname ??= string.Empty;
            if (model.Error.StatusCode != HttpStatusCode.OK)
                return Index(model);
            using (SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData")))
            {
                using var command = connection.CreateCommand();
                {
                    
                    connection.Open();
                    RewriteAccount(model);
                    if (model.NickName != GetNickname())
                        RenameAccountTable(model);
                    Models.TempData.AccountTableName = model.NickName;
                }
            }
            return RedirectToAction("Index", "PersonalAccount");
        }

        private ErrorModel CheckAndSetError(EditAccountModel model)
        {
            if (model.CheckBlankFields().StatusCode != HttpStatusCode.OK)
                return model.CheckBlankFields();
            else if (CheckNickName(model).StatusCode != HttpStatusCode.OK)
                return model.CheckNickName<EditAccountModel>();
            else if (CheckEmailAlreadyExist(model).StatusCode != HttpStatusCode.OK)
                return model.CheckEmailAlreadyExist<EditAccountModel>();
            return new ErrorModel();
        }

        private void RewriteAccount(EditAccountModel model)
        {
            using (SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData")))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"UPDATE AuthData SET " +
                                          $"Name = '{model.Name.Trim()}', " +
                                          $"Surname = '{model.Surname.Trim()}', " +
                                          $"DateOfBirth = '{model.DateOfBirth}', " +
                                          $"Email = '{model.Email.Trim()}', " +
                                          $"Password = '{model.Password.Trim()}', " +
                                          $"NickName = '{model.NickName.Trim()}' " +
                                          $"WHERE UserID = '{model.Id}'";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private void RenameAccountTable(EditAccountModel model)
        {
            string sqlCommand = $"ALTER TABLE {Models.TempData.AccountTableName} RENAME TO {model.NickName.Trim()}";
            using (SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData")))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = sqlCommand;
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private ErrorModel CheckEmailAlreadyExist(IVerifiableData model)
        {
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            connection.Open();
            EditAccountModel oldDataModel =
                AccountSql.InstantiateAccountModel<EditAccountModel>("UserID", model.Id.ToString());
            ErrorModel errorModel = new();
            string email = model.Email;
            string tableName = "AuthData";
            using SqliteCommand command = new($"Select * from {tableName} where Email = '{model.Email}'", connection);
            using(SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read() && oldDataModel.Id != model.Id)
                    errorModel = new ErrorModel(HttpStatusCode.BadRequest, "This email is already in use");
            }
            return errorModel;
        }

        private ErrorModel CheckNickName(IVerifiableData model)
        {
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            ErrorModel errorModel = new();
            connection.Open();
            EditAccountModel oldDataModel =
                AccountSql.InstantiateAccountModel<EditAccountModel>("UserID", model.Id.ToString());
            using SqliteCommand command = new($"SELECT name " +
                                              $"FROM sqlite_master " +
                                              $"WHERE type='table' " +
                                              $"AND name='{model.NickName}'", connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read() && oldDataModel.Id != model.Id)
                    errorModel = new ErrorModel(HttpStatusCode.Conflict, "This nickname already used");
            }

            return errorModel;
        }

        private string GetNickname()
        {
            return Models.TempData.AccountTableName;
        }
    }
}
