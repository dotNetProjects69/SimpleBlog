using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using static SimpleBlog.Validators.Base.TypeTransformer;

namespace SimpleBlog.Validators.ValidatorType
{
    public class PasswordMustNotEmpty : Validator<IAccountModelPart>
    {
        private protected override IErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<IPassword>(baseModel);
            bool result = string.IsNullOrWhiteSpace(model.Password);
            return result
                ? new(HttpStatusCode.BadRequest, "Password field is blank")
                : new ErrorModel();
        }
    }
}
