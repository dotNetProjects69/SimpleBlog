using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models.Account;

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
            AccountInfoModel accountModel = SqlExtensions
                                            .InstantiateAccountModel<AccountInfoModel>("nickname",
                                                                                       Models.TempData.AccountTableName);
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
            model = SqlExtensions.InstantiateAccountModel<EditAccountModel>("NickName",
                                                                            Models.TempData.AccountTableName);
            return View("EditAccount", model);
        }

        private void DeleteAccountTable()
        {
            bool result = SqlExtensions.DropTable(Models.TempData.AccountTableName, out string error);
            if (!result)
                Console.WriteLine(error);
        }

        private void DeleteAccountData()
        {
            string sqlCommand = $"DELETE FROM AuthData WHERE NickName = '{Models.TempData.AccountTableName}'";
            SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
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
