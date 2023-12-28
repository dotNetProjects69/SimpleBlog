using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models.Authentication;
using SimpleBlog.Validators.ValidatorType;
using static SimpleBlog.Controllers.Extensions.AccountSql;
using static SimpleBlog.Controllers.SignInControllers.PasswordComparer;

namespace SimpleBlog.Controllers.SignInControllers
{
    public class SignInByEmail : SignInController
    {
        public SignInByEmail(ILogger<SignInController> logger, IConfiguration configuration)
            : base(logger, configuration) { }

        public override IActionResult Index(SignInModel? model = null)
        {
            _model = model ?? new();
            return View(_signInByEmailPagePath, model);
        }

        public IActionResult LogIn(SignInModel? model)
        {
            _model = model ?? new();
            ValidateEmail();
            if (_model.Error.StatusCodeIsOk()) 
                _model.Error = CompareInputPasswordByEmail(model);
            if (_model.Error.StatusCodeIsNotOk())
                return Index(_model);
            SetNicknameByEmail();
            SetCurrentAccountIdToGlobal();
            return RedirectToAction("Index", "Posts");
        }

        public IActionResult LogInByNickname()
        {
            return RedirectToAction("Index", "SignInByNickname");
        }

        private void ValidateEmail()
        {
            _model.Error = new EmailMustExist().Validate(_model);
        }

        private void SetNicknameByEmail()
        {
            _model.Nickname = SelectFromTableByWhere("NickName", "Email", _model.Email)[0];
        }
    }
}
