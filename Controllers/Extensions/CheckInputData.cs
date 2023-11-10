using Microsoft.Data.Sqlite;
using SimpleBlog.Models;
using SimpleBlog.Models.Account;
using SimpleBlog.Models.Authentification;
using System.Net;

namespace SimpleBlog.Controllers.Extensions
{
    public static class CheckInputData
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
                                                                    .AddJsonFile("appsettings.json")
                                                                    .Build();

        public static IConfiguration Configuration => _configuration;

        internal static ErrorModel CheckBlankFields(this IVerifiableData model)
        {
            bool result = string.IsNullOrWhiteSpace(model.Name) || !IsValidEmail(model.Email) ||
                    string.IsNullOrWhiteSpace(model.NickName) || string.IsNullOrWhiteSpace(model.Password);
            ErrorModel errorModel = new();
            if (result)
                errorModel = new(HttpStatusCode.BadRequest, "Some required fields are blank");
            return errorModel;
        }

        internal static ErrorModel CheckNickName<T>(this IVerifiableData model) where T : class, IAccountModel, new()
        {
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            ErrorModel errorModel = new();
            connection.Open();
            T oldDataModel =
                SqlExtensions.InstantiateAccountModel<T>("UserID", model.Id.ToString());
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

        internal static ErrorModel CheckEmailAlreadyExist<T>(this IVerifiableData newDataModel) where T : class, IAccountModel, new()
        {
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            connection.Open();
            T oldDataModel =
                SqlExtensions.InstantiateAccountModel<T>("UserID", newDataModel.Id.ToString());
            ErrorModel errorModel = new();
            string email = newDataModel.Email;
            string tableName = "AuthData";
            using SqliteCommand command = new($"Select * from {tableName} where Email = '{newDataModel.Email}'", connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read() && oldDataModel.Id != newDataModel.Id)
                    errorModel = new ErrorModel(HttpStatusCode.BadRequest, "This email is already in use");
            }
            return errorModel;
        }

        internal static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return false; // suggested by @TK-421

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
