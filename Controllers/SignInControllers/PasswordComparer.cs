using SimpleBlog.Models;
using SimpleBlog.Models.Authentication;
using System.Net;
using static SimpleBlog.Controllers.Extensions.AccountSql;

namespace SimpleBlog.Controllers.SignInControllers
{
    public abstract class PasswordComparer
    {
        internal static void CompareInputPasswordByEmail(SignInModel model)
        {
            IReadOnlyList<string> resultList = 
                SelectFromTableByWhere("Password", "Email", model.Email);
            string result = resultList.Any() ? resultList[0] : string.Empty;
            CheckInputPassword(model, result);
        }

        internal static void CompareInputPasswordByNickname(SignInModel model)
        {
            IReadOnlyList<string> resultList = 
                SelectFromTableByWhere("Password", "NickName", model.Nickname);
            string result = resultList.Any() ? resultList[0] : string.Empty;
            CheckInputPassword(model, result);
        }

        private static void CheckInputPassword(SignInModel model, string result)
        {
            ErrorModel errorModel = new ();
            string password = model.Password;
            if (result != password)
            {
                errorModel.StatusCode = HttpStatusCode.BadRequest;
                errorModel.Message = "Password not valid";
            }
            model.Error = errorModel;
        }
    }
}
