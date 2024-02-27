using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Models.Account;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;
using static SimpleBlog.Shared.SessionHandler;
using static SimpleBlog.Shared.GlobalParams;
using SimpleBlog.Shared;

namespace SimpleBlog.Controllers
{
    public class PersonalAccountController : Controller
    {
        private readonly ILogger<PersonalAccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHandler _sessionHandler;

        public PersonalAccountController(ILogger<PersonalAccountController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHandler = new SessionHandler();
        }

        public IActionResult Index()
        {
            if (GetCurrentAccountId() == string.Empty)
                return RedirectToAction("Index", "SignUp");
            AccountInfoModel accountModel = InstantiateAccountModelOrEmpty<AccountInfoModel>("UserID", GetCurrentAccountId());
            return View(accountModel);
        }

        
        public IActionResult Delete()
        {
            DeleteAccountData();
            DeleteAccountTable();
            SetAccountId(string.Empty);
            return RedirectToAction("Index", "SignUp");
        }

        
        public IActionResult LogOut()
        {
            SetAccountId(string.Empty);
            return RedirectToAction("Index", "SignUp");
        }

        
        public IActionResult Update(EditAccountModel model)
        {
            model = InstantiateAccountModelOrEmpty<EditAccountModel>("UserID", GetCurrentAccountId());
            return View("EditAccount", model);
        }

        private void DeleteAccountTable()
        {
            DropTable(GetCurrentAccountId());
        }

        private void DeleteAccountData()
        {
            string sqlCommand = $"DELETE FROM AuthData WHERE UserID = '{GetCurrentAccountId()}'";
            SqliteConnection connection = new(GetAccountsDataPath());
            SqliteCommand command = new(sqlCommand, connection);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private protected virtual void SetAccountId(string value)
        {
            _sessionHandler.SessionOwnerId = value;
        }

        private string GetCurrentAccountId()
        {
            return _sessionHandler.SessionOwnerId;
        }
        
    }
}
