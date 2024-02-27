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
    public class PasswordHasNoGaps : Validator<IAccountModelPart>
    {
        private protected override IErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<IPassword>(baseModel);
            bool result = model.Password.Contains(' ');

            return result
                ? new(HttpStatusCode.BadRequest, "Password must not contain gaps")
                : new ErrorModel();
        }
    }
}
