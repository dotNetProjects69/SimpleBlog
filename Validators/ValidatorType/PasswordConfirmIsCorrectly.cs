using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using System.Runtime.CompilerServices;
using static SimpleBlog.Validators.Base.TypeTransformer;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Validators.ValidatorType
{
    public class PasswordConfirmIsCorrectly : Validator<IAccountModelPart>
    {
        private protected override IErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<IConfirmedPassword>(baseModel);
            bool result = model.Password != model.ConfirmedPassword;
            return result
                ? new(HttpStatusCode.BadRequest, "Fields Password and Confirm Password are not same")
                : new ErrorModel();
        }
    }
}
