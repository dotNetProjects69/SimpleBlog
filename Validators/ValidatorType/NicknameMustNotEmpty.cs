using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using static SimpleBlog.Validators.Base.TypeTransformer;

namespace SimpleBlog.Validators.ValidatorType
{
    public class NicknameMustNotEmpty : Validator<IAccountModelPart>
    {
        private protected override IErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            INickname? model = TryTransformTo<INickname>(baseModel);
            bool result = string.IsNullOrWhiteSpace(model.Nickname);
            return result
                ? new(HttpStatusCode.BadRequest, "Nickname field is blank")
                : ErrorModel.Success;
        }
    }
}
