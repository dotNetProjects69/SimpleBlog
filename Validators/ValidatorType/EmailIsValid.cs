using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using static SimpleBlog.Validators.Base.TypeTransformer;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Validators.ValidatorType
{
    public class EmailIsValid :  Validator<IAccountModelPart>
    {
        private protected override IErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<IEmail>(baseModel);
            bool result = IsValidEmail(model.Email);
            return !result
                ? new(HttpStatusCode.BadRequest, "Email is not valid")
                : new ErrorModel();
        }

        private static bool IsValidEmail(string email)
        {
            string trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith('.'))
                return false; // suggested by @TK-421

            try
            {
                MailAddress emailAddress = new (email);
                return emailAddress.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
