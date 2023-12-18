using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models.Authentication;

namespace SimpleBlog.Controllers.SignInControllers
{
    public class SignInByNickname : SignInController
    {
        public SignInByNickname(ILogger<SignInController> logger, IConfiguration configuration)
            : base(logger, configuration) { }

        public override IActionResult Index(SignInModel? model = null)
        {
            model ??= new();
            return View(_signInByNicknamePagePath, model);
        }

        public IActionResult LogIn(SignInModel model)
        {
            model ??= new();
            ValidateNickName(model);
            if (StatusCodeIsOk(model))
                ValidateInputPassword(model);
            if (StatusCodeIsOk(model))
            {
                SetAccountTableName(model);
                SetCurrentGuid(model);
                return RedirectToAction("Index", "Posts");
            }
            return Index(model);
        }

        public IActionResult LogInByEmail()
        {
            return RedirectToAction("Index", "SignInByEmail");
        }

        private void SetCurrentGuid(SignInModel model)
        {
            Models.TempData.AccountId = new Guid(GetGuidByNickname(model));
        }

        private void ValidateNickName(SignInModel signInModel)
        {
            signInModel.Error = signInModel.CheckNickNameDoesNotExist();
        }

        private string GetGuidByNickname(SignInModel model)
        {
            return GetGuid("Nickname", model.NickName);
        }
    }
}
