using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Models;
using System.Data;
using System.Globalization;
using System.Reflection;

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
            AccountModel accountModel = InstantiateAccountModel();
            return View(accountModel);
        }

        public IActionResult Delete()
        {
            DeleteAccountData();
            DeleteAccountTable();
            Models.TempData.AccountTableName = string.Empty;
            return RedirectToAction("Index", "SignUp");
        }

        private void DeleteAccountTable()
        {
            string sqlCommand = $"DROP TABLE '{Models.TempData.AccountTableName}'";
            SqliteConnection connection = new(_configuration.GetConnectionString(""));
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

        private AccountModel InstantiateAccountModel()
        {
            AccountModel model = new AccountModel();
            string sqlCommand = $"SELECT * FROM AuthData WHERE nickname = '{Models.TempData.AccountTableName}'";
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            using SqliteCommand command = new(sqlCommand, connection);
            connection.Open();
            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows) return model;
                reader.Read();

                ReadData(model, reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return model;
        }

        private static void ReadData(AccountModel model, SqliteDataReader reader)
        {
            DateTimeFormatInfo dateTimeFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
            string dateFormat = dateTimeFormatInfo.ShortDatePattern;

            model.Name = reader.GetString(1);
            model.Surname = reader.GetString(2);
            _ = DateOnly.TryParse(reader.GetString(4), 
                                  CultureInfo.CurrentCulture, 
                                  DateTimeStyles.None, 
                                  out DateOnly dateOnly);
            model.DateOfBirth = dateOnly;
            model.Email = reader.GetString(4);
            model.NickName = reader.GetString(7);

            Console.WriteLine($"Name - {model.Name}\n" +
                              $"Surname - {model.Surname}\n" +
                              $"Date of birth - {model.DateOfBirth}\n" +
                              $"Email - {model.Email}\n" +
                              $"Nicknmae - {model.NickName}");
        }
    }
}
