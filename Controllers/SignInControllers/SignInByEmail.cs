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
                ValidateInputPassword(model);
            if (!StatusCodeIsOk(model))
                return Index(model);
            SetAccountTableName(model);
            SetCurrentGuid(model);
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

        private void SetCurrentGuid(SignInModel model)
        {
            Models.TempData.AccountId = new(GetGuidByEmail(model));
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
            base.SetAccountTableName(model);
        }

        private protected override void ValidateInputPassword(SignInModel model)
        {
            SetNickname(model);
            base.ValidateInputPassword(model);
        }
    }
}
