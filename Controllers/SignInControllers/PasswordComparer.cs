using SimpleBlog.Models;
using SimpleBlog.Models.Authentication;
using SimpleBlog.Models.Interfaces;
using System.Net;
using System.Runtime.CompilerServices;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Controllers.SignInControllers
{
    public abstract class PasswordComparer
    {
        internal static IErrorModel CompareInputPasswordByEmail(SignInModel model)
        {
            return CheckInputPassword(model, "Email", model.Email);
        }

        internal static IErrorModel CompareInputPasswordByNickname(SignInModel model)
        {
            return CheckInputPassword(model, "NickName", model.Nickname);
        }

        private static IErrorModel CheckInputPassword(SignInModel model, string filterParam, string filterName)
        {
            IErrorModel errorModel = new ErrorModel();
            IReadOnlyCollection<string> resultList = 
                SelectFromTableByWhere("Password", filterParam, filterName);
            string result = resultList.Any() ? (resultList.ElementAtOrDefault(0) ?? string.Empty) : string.Empty;
            string password = model.Password;
            if (result != password)
            {
                errorModel.SetErrorInfo(HttpStatusCode.BadRequest, "Password not valid");
            }
            return errorModel;
        }
    }
}
