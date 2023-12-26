using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using static SimpleBlog.Validators.Base.TypeTransformer;

namespace SimpleBlog.Validators.ValidatorType
{
    public class NameMustNotEmpty : Validator<IAccountModelPart>
    {
        private protected override ErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<IName>(baseModel);
            bool result = string.IsNullOrWhiteSpace(model.Name);
            return result
                ? new(HttpStatusCode.BadRequest, "Name field is blank")
                : new();
        }
    }
}
