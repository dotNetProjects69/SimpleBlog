using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models.Authentication;
using SimpleBlog.Validators.ValidatorType;
using static SimpleBlog.Controllers.SignInControllers.PasswordComparer;

namespace SimpleBlog.Controllers.SignInControllers
{
    public class SignInByNickname : SignInController
    {
        public SignInByNickname(ILogger<SignInController> logger, IConfiguration configuration)
            : base(logger, configuration) { }

        public override IActionResult Index(SignInModel? model = null)
        {
            _model = model ?? new();
            return View(_signInByNicknamePagePath, _model);
        }

        public IActionResult LogIn(SignInModel? model)
        {
            _model = model ?? new();
            ValidateNickName(_model);
            if (StatusCodeIsOk(_model))
                _model.Error = CompareInputPasswordByNickname(_model);
            if (StatusCodeIsOk(_model))
            {
                SetCurrentAccountIdToGlobal();
                return RedirectToAction("Index", "Posts");
            }
            return Index(_model);
        }

        public IActionResult LogInByEmail()
        {
            return RedirectToAction("Index", "SignInByEmail");
        }

        private void ValidateNickName(SignInModel signInModel)
        {
            signInModel.Error = new NickNameMustUsed().Validate(signInModel);
        }
    }
}
