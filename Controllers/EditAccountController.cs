using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models;
using SimpleBlog.Models.Account;
using System.Net;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using SimpleBlog.Validators.ValidatorType;
using static SimpleBlog.Models.TempData;
using static SimpleBlog.Shared.GlobalParams;
using SimpleBlog.Models.Authentication;

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
                }
            }
            new NotFoundResult();
            return RedirectToAction("Index", "PersonalAccount");
        }

        

        private ErrorModel CheckAndSetError(EditAccountModel model)
        {
            ValidationChain<IAccountModelPart> chain = new();
            chain
                .SetNext(new ModelHasNoBlankFields())
                .SetNext(new NicknameCharsIsCorrect())
                .SetNext(new PasswordLengthEnough());

            return chain.Validate(model);
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
                                          $"NickName = '{model.Nickname.Trim()}' " +
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

        private protected virtual void SetAccountId(EditAccountModel model)
        {
            HttpContext.Session.SetString(AccountIdSessionKey, model.Id.ToString());
        }

        private string GetCurrentAccountId()
        {
            return HttpContext.Session.GetString(AccountIdSessionKey) ?? "";
        }
    }
}
