using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using System.Runtime.CompilerServices;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;
using static SimpleBlog.Validators.Base.TypeTransformer;

namespace SimpleBlog.Validators.ValidatorType
{
    public class EmailMustNotExist : Validator<IAccountModelPart>
    {
        private protected override IErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<IEmail>(baseModel);
            bool result = AccountExist("Email", model.Email);

            return result
                ? new(HttpStatusCode.Conflict, "This email is already in use")
                : new ErrorModel();
        }
    }
}
