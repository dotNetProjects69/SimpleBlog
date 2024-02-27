using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;
using static SimpleBlog.Validators.Base.TypeTransformer;

namespace SimpleBlog.Validators.ValidatorType
{
    public class EmailMustExist : Validator<IAccountModelPart>
    {
        private protected override IErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<IEmail>(baseModel);
            bool result = !AccountExist("Email", model.Email);
            return result
                ? new(HttpStatusCode.NotFound, "An account with such an email does not exist")
                : new ErrorModel();
        }
    }
}
