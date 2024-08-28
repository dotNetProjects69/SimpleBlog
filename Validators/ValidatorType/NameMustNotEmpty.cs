using System.Net;
using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using static SimpleBlog.Validators.Base.TypeTransformer;

namespace SimpleBlog.Validators.ValidatorType
{
    public class NameMustNotEmpty : Validator<IAccountModelPart>
    {
        private protected override IErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            IName model = TryTransformTo<IName>(baseModel);
            bool result = string.IsNullOrWhiteSpace(model.Name);
            return result
                ? new(HttpStatusCode.BadRequest, "Name field is blank")
                : ErrorModel.Success;
        }
    }
}
