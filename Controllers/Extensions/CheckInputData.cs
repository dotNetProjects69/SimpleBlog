using Microsoft.Data.Sqlite;
using SimpleBlog.Models;
using SimpleBlog.Models.Account;
using SimpleBlog.Models.Authentification;
using System.Net;
using System.Net.Mail;
using System.Reflection.PortableExecutable;
using static System.Console;

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

        internal static ErrorModel CheckNickName<T>(this IVerifiableData newDataModel) where T : class, IAccountModel, new()
        {
            ErrorModel errorModel = new();
            T oldDataModel =
                AccountSql.InstantiateAccountModel<T>("NickName", newDataModel.NickName);
            if (!oldDataModel.Id.Equals(new Guid()))
                errorModel = new ErrorModel(HttpStatusCode.Conflict, "This nickname already used");

            return errorModel;
        }

        internal static ErrorModel CheckEmailAlreadyExist<T>(this IVerifiableData newDataModel) 
            where T : class, IAccountModel, new()
        {
            T oldDataModel =
                AccountSql.InstantiateAccountModel<T>("Email", newDataModel.Email);
            ErrorModel errorModel = new();
            if (oldDataModel.Email != string.Empty)
                errorModel = new ErrorModel(HttpStatusCode.BadRequest, "This email is already in use");
            return errorModel;
        }

        internal static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return false; // suggested by @TK-421

            try
            {
                MailAddress emailAddress = new MailAddress(email);
                return emailAddress.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
