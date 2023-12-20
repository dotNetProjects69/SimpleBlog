using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models;
using SimpleBlog.Models.Account;
using System.Net;
using static SimpleBlog.Models.TempData;
using static SimpleBlog.Shared.GlobalParams;

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
            using (SqliteConnection connection = new(GetAccountsDataPath()))
            {
                using var command = connection.CreateCommand();
                {
                    
                    connection.Open();
                    RewriteAccount(model);
                    if (model.NickName != GetCurrentNickname())
                        RenameAccountTable(model);
                    SetCurrentNickname(model);
                }
            }
            new NotFoundResult();
            return RedirectToAction("Index", "PersonalAccount");
        }

        

        private ErrorModel CheckAndSetError(EditAccountModel model)
        {
            if (model.DetectBlankFields().StatusCode != HttpStatusCode.OK)
                return model.DetectBlankFields();
            else if (model.CheckNickNameAlreadyUsed().StatusCode != HttpStatusCode.OK)
                return model.CheckNickNameAlreadyUsed();
            else if (model.CheckEmailAlreadyExist().StatusCode != HttpStatusCode.OK)
                return model.CheckEmailAlreadyExist();
            return new ErrorModel();
        }

        private void RewriteAccount(EditAccountModel model)
        {
            using (SqliteConnection connection = new(GetAccountsDataPath()))
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
            string sqlCommand = $"ALTER TABLE {GetCurrentNickname()} RENAME TO {model.NickName.Trim()}";
            using (SqliteConnection connection = new(GetAccountsDataPath()))
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

        private void SetCurrentNickname(EditAccountModel model)
        {
            HttpContext.Session.SetString(NicknameSessionKey, model.NickName);
        }

        private string GetCurrentNickname()
        {
            return HttpContext.Session.GetString(NicknameSessionKey) ?? "";
        }
    }
}
