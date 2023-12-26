using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using static SimpleBlog.Validators.Base.TypeTransformer;

namespace SimpleBlog.Validators.ValidatorType
{
    public class NicknameMustNotEmpty : Validator<IAccountModelPart>
    {
        private protected override ErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<INickname>(baseModel);
            bool result = string.IsNullOrWhiteSpace(model.Nickname);
            return result
                ? new(HttpStatusCode.BadRequest, "Nickname field is blank")
                : new();
        }
    }
}
