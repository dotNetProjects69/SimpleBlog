using SimpleBlog.Models;
using SimpleBlog.Models.Account;
using SimpleBlog.Models.Authentification;
using System.Net;
using System.Net.Mail;

namespace SimpleBlog.Controllers.Extensions
{
    public static class ValidationInputData
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
                                                                    .AddJsonFile("appsettings.json")
                                                                    .Build();

        public static IConfiguration Configuration => _configuration;

        internal static ErrorModel DetectBlankFields(this IVerifiableFull model)
        {
            bool result = string.IsNullOrWhiteSpace(model.Name) || !IsValidEmail(model.Email) ||
                    string.IsNullOrWhiteSpace(model.NickName) || string.IsNullOrWhiteSpace(model.Password);
            ErrorModel errorModel = new();

            if (result)
                errorModel = new(HttpStatusCode.BadRequest, "Some required fields are blank");

            return errorModel;
        }

        internal static ErrorModel CheckNickNameAlreadyUsed(this IVerifiableCore newDataModel)
        {
            ErrorModel errorModel = new();

            if (AccountExist("NickName", newDataModel.NickName))
                errorModel = new ErrorModel(HttpStatusCode.Conflict, "This nickname already used");

            return errorModel;
        }

        internal static ErrorModel CheckNickNameDoesNotExist(this IVerifiableCore newDataModel)
        {
            ErrorModel errorModel = new();

            if (!AccountExist("NickName", newDataModel.NickName))
                errorModel = new ErrorModel(HttpStatusCode.NotFound, "An account with such a nickname does not exist");

            return errorModel;
        }

        internal static ErrorModel CheckEmailAlreadyExist(this IVerifiableCore newDataModel)
        {
            ErrorModel errorModel = new();

            if (AccountExist("Email", newDataModel.Email))
                errorModel = new ErrorModel(HttpStatusCode.Conflict, "This email is already in use");

            return errorModel;
        }

        internal static ErrorModel CheckEmailDoesNotExist(this IVerifiableCore newDataModel)
        {
            ErrorModel errorModel = new();

            if (!AccountExist("Email", newDataModel.Email))
                errorModel = new ErrorModel(HttpStatusCode.Conflict, "An account with such an email does not exist");

            return errorModel;
        }

        private static bool AccountExist(string paramName, string paramValue)
        {
            EditAccountModel model = AccountSql.InstantiateAccountModel<EditAccountModel>(paramName, paramValue);
            return model.Id != new Guid();
        }

        internal static bool IsValidEmail(string email)
        {
            string trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith('.'))
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
