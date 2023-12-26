using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using System.Runtime.CompilerServices;
using static SimpleBlog.Controllers.Extensions.AccountSql;
using static SimpleBlog.Validators.Base.TypeTransformer;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Validators.ValidatorType
{
    public class NickNameMustUsed : Validator<IAccountModelPart>   
    {
        private protected override ErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<INickname>(baseModel);
            bool result = !AccountExist("NickName", model.Nickname);

            return result
                ? new(HttpStatusCode.NotFound, "An account with such a nickname does not exist")
                : new();
        }
    }
}
