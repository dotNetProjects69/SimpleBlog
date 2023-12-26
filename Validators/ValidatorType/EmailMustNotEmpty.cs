using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using static SimpleBlog.Validators.Base.TypeTransformer;

namespace SimpleBlog.Validators.ValidatorType
{
    public class EmailMustNotEmpty : Validator<IAccountModelPart>
    {
        private protected override ErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<IEmail>(baseModel);
            bool result = string.IsNullOrWhiteSpace(model.Email);
            return result
                ? new(HttpStatusCode.BadRequest, "Email field is blank")
                : new();
        }
    }
}
