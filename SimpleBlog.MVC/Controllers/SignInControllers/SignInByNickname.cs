using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Common.DTOs;
using SimpleBlog.MVC.Models;
using SimpleBlog.MVC.Validation.Validation.Enums;
using SimpleBlog.MVC.Validation.ValidationV2.ViewModel;
using SimpleBlog.MVC.Validation.ViewModels;
using SimpleBlog.MVC.Validation.ViewModels.Authentication;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.MVC.Controllers.SignInControllers
{
    public class SignInByNickname : SignInController
    {
        private CreatingAccountModelValidator _accountValidator;
        
        public SignInByNickname(ILogger<SignInController> logger, 
            IConfiguration configuration,
            IAccountService accountService, 
            CreatingAccountModelValidator accountValidator)
            : base(logger, configuration, accountService)
        {
            _accountValidator = accountValidator;
        }

        public override IActionResult Index(SignInModel? model = null)
        {
            model ??= new();
            return View(SignInByNicknamePagePath, new MainViewModel<SignInModel>(model));
        }

        public async Task<IActionResult> LogIn(SignInModel? model)
        {
            model ??= new();
            ValidationResult? validationResult = await _accountValidator.ValidateAsync(model);
            
            MainViewModel<SignInModel> mainViewModel = new(model, validationResult);

            if (!validationResult.IsValid && validationResult.Errors.Any(errorModel => errorModel is
                {
                    PropertyName: not nameof(model.Email)
                }))
                return View(SignInByNicknamePagePath, mainViewModel);

            AccountDto? accountDto = (await AccountService.GetAllModelsByNickname(model.Nickname)).FirstOrDefault();
            SaveAccountIdToSession(accountDto!.AccountId);
            
            return RedirectToAction("Index", "Posts");
        }

        public IActionResult LogInByEmail()
        {
            return RedirectToAction("Index", "SignInByEmail");
        }
    }
}