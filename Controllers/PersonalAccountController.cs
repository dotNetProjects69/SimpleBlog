using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models.Account;
using System.Globalization;

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
            AccountInfoModel accountModel = InstantiateAccountModel<AccountInfoModel>();
            return View(accountModel);
        }

        [HttpPost]
        public IActionResult Delete()
        {
            DeleteAccountData();
            DeleteAccountTable();
            Models.TempData.AccountTableName = string.Empty;
            return RedirectToAction("Index", "SignUp");
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            Models.TempData.AccountTableName = string.Empty;
            return RedirectToAction("Index", "SignUp");
        }

        [HttpPost]
        public IActionResult Update(EditAccountModel model)
        {
            model = InstantiateAccountModel<EditAccountModel>();
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
            string sqlCommand = $"DELETE FROM AuthData WHERE nickname = '{Models.TempData.AccountTableName}'";
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

        private T InstantiateAccountModel<T>() where T : class, IAccountModel, new()
        {
            T model = new();
            string sqlCommand = $"SELECT * FROM AuthData WHERE nickname = '{Models.TempData.AccountTableName}'";
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            using SqliteCommand command = new(sqlCommand, connection);
            connection.Open();
            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows) return model;
                reader.Read();


                // use Visitor pattern
                if (model is EditAccountModel)
                    model = SetData(model as EditAccountModel ?? new(), reader) as T ?? new T();
                else 
                    model = SetData(model, reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return model;
        }

        private static T SetData<T>(T model, SqliteDataReader reader) where T : IAccountModel
        {
            model.Id = new(reader.GetString(6));
            model.Name = reader.GetString(1);
            model.Surname = reader.GetString(2);
            _ = DateOnly.TryParse(reader.GetString(4), 
                                  CultureInfo.CurrentCulture, 
                                  DateTimeStyles.None, 
                                  out DateOnly dateOnly);
            model.DateOfBirth = dateOnly;
            model.Email = reader.GetString(4);
            model.NickName = reader.GetString(7);
            return model;
        }

        private static EditAccountModel SetData(EditAccountModel model, SqliteDataReader reader)
        {
            model = SetData<EditAccountModel>(model, reader);
            model.Password = reader.GetString(5);
            return model;
        }
    }
}
