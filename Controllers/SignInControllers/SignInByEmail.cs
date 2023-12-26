using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models.Authentication;
using SimpleBlog.Validators.ValidatorType;
using static SimpleBlog.Controllers.SignInControllers.PasswordComparer;

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
            if (model.Error.StatusCodeIsOk()) 
                CompareInputPasswordByEmail(model);
            if (model.Error.StatusCodeIsNotOk())
                return Index(model);
            SetCurrentNickname(model);
            return RedirectToAction("Index", "Posts");
        }

        public IActionResult LogInByNickname()
        {
            return RedirectToAction("Index", "SignInByNickname");
        }

        private void ValidateEmail(SignInModel model)
        {
            model.Error = new EmailMustExist().Validate(model);
        }
    }
}
