using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static SimpleBlog.Validators.Base.TypeTransformer;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Validators.ValidatorType
{
    public class NicknameCharsIsCorrect : Validator<IAccountModelPart>
    {
        private protected override ErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            var model = TryTransformTo<INickname>(baseModel);
            string pattern = "^[a-z0-9_]+$";
            bool isValid = Regex.IsMatch(model.Nickname, pattern);
            return isValid
                ? new ErrorModel()
                : new(HttpStatusCode.BadRequest, "Nickname must contains only a-z, 0-9 and _");
        }
    }
}
