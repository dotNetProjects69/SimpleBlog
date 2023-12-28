using SimpleBlog.Models;
using SimpleBlog.Models.Authentication;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using System.Net;
using System.Runtime.CompilerServices;
using static SimpleBlog.Controllers.Extensions.AccountSql;

[assembly:InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Controllers.SignInControllers
{
    public abstract class PasswordComparer
    {
        internal static ErrorModel CompareInputPasswordByEmail(SignInModel model)
        {
            return CheckInputPassword(model, "Email", model.Email);
        }

        internal static ErrorModel CompareInputPasswordByNickname(SignInModel model)
        {
            return CheckInputPassword(model, "NickName", model.Nickname);
        }

        private static ErrorModel CheckInputPassword(SignInModel model, string filterParam, string filterName)
        {
            ErrorModel errorModel = new();
            IReadOnlyList<string> resultList = 
                SelectFromTableByWhere("Password", filterParam, filterName);
            string result = resultList.Any() ? resultList[0] : string.Empty;
            string password = model.Password;
            if (result != password)
            {
                errorModel.StatusCode = HttpStatusCode.BadRequest;
                errorModel.Message = "Password not valid";
            }
            return errorModel;
        }
    }
}
