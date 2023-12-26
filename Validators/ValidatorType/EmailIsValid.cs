using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using System.Net.Mail;
using static SimpleBlog.Validators.Base.TypeTransformer;

namespace SimpleBlog.Validators.ValidatorType
{
    public class EmailIsValid :  Validator<IAccountModelPart>
    {
        private protected override ErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<IEmail>(baseModel);
            bool result = IsValidEmail(model.Email);
            return !result
                ? new(HttpStatusCode.BadRequest, "Email is not valid")
                : new();
        }

        private static bool IsValidEmail(string email)
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
