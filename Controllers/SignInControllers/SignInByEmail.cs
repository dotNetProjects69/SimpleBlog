using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models.Authentification;

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
            if (StatusCodeIsOK(model))
                ValidateInputPassword(model);
            if (StatusCodeIsOK(model))
            {
                SetAccountTableName(model);
                SetCurrentGuid(model);
                return RedirectToAction("Index", "Posts");
            }
            return Index(model);
        }

        private static void SetNickname(SignInModel model)
        {
            model.NickName = AccountSql.SelectFromTable("NickName", "Email", model.Email).First();
        }

        public IActionResult LogInByNickname()
        {
            return RedirectToAction("Index", "SignInByNickname");
        }

        private void SetCurrentGuid(SignInModel model)
        {
            Models.TempData.AccountId = new Guid(GetGuidByEmail(model));
        }

        private void ValidateEmail(SignInModel model)
        {
            model.Error = model.CheckEmailDoesNotExist();
        }

        private string GetGuidByEmail(SignInModel model)
        {
            return GetGuid("Email", model.Email);
        }

        private protected override void SetAccountTableName(SignInModel model)
        {
            SetNickname(model);
            base.SetAccountTableName(model);
        }
    }
}
