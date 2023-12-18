using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Models.Account;
using static SimpleBlog.Controllers.Extensions.AccountSql;
using static SimpleBlog.Models.TempData;
using static SimpleBlog.Shared.GlobalParams;

namespace SimpleBlog.Controllers
{
    public class PersonalAccountController : Controller
    {
        private readonly ILogger<PersonalAccountController> _logger;
        private readonly IConfiguration _configuration;

        public PersonalAccountController(ILogger<PersonalAccountController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (Models.TempData.AccountTableName == string.Empty)
                return RedirectToAction("Index", "SignUp");
            AccountInfoModel accountModel = InstantiateAccountModelOrEmpty<AccountInfoModel>("nickname", AccountTableName);
            return View(accountModel);
        }

        
        public IActionResult Delete()
        {
            DeleteAccountData();
            DeleteAccountTable();
            Models.TempData.AccountTableName = string.Empty;
            return RedirectToAction("Index", "SignUp");
        }

        
        public IActionResult LogOut()
        {
            Models.TempData.AccountTableName = string.Empty;
            return RedirectToAction("Index", "SignUp");
        }

        
        public IActionResult Update(EditAccountModel model)
        {
            model = InstantiateAccountModelOrEmpty<EditAccountModel>("NickName",
                                                                      Models.TempData.AccountTableName);
            return View("EditAccount", model);
        }

        private void DeleteAccountTable()
        {
            DropTable(Models.TempData.AccountTableName);
        }

        private void DeleteAccountData()
        {
            string sqlCommand = $"DELETE FROM AuthData WHERE NickName = '{Models.TempData.AccountTableName}'";
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
        
    }
}
