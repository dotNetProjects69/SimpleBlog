using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models.Authentication;
using static SimpleBlog.Controllers.Extensions.AccountSql;

namespace SimpleBlog.Controllers.SignInControllers
{
    public class SignInByEmail : SignInController
    {
        public SignInByEmail(ILogger<SignInController> logger, IConfiguration configuration)
            : base(logger, configuration) { }

        public override IActionResult Index(SignInModel? model = null)
        {
            model ??= new();
            return View(_signInByEmailPagePath, model);
        }

        public IActionResult LogIn(SignInModel model)
        {
            model ??= new();
            ValidateEmail(model);
            if (StatusCodeIsOk(model))
            {
                SetNickname(model);
                ValidateInputPassword(model);
            }
            if (!StatusCodeIsOk(model))
                return Index(model);
            SetCurrentNickname(model);
            return RedirectToAction("Index", "Posts");
        }

        private static void SetNickname(SignInModel model)
        {
            model.NickName = SelectFromTableByWhere("NickName", "Email", model.Email)[0];
        }

        public IActionResult LogInByNickname()
        {
            return RedirectToAction("Index", "SignInByNickname");
        }

        private void ValidateEmail(SignInModel model)
        {
            model.Error = model.CheckEmailDoesNotExist();
        }
    }
}
