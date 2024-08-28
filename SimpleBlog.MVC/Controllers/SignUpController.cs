using System.Runtime.CompilerServices;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Common.DTOs;
using SimpleBlog.MVC.Extensions;
using SimpleBlog.MVC.Models;
using SimpleBlog.MVC.Shared;
using SimpleBlog.MVC.Validation.ValidationV2.ViewModel;
using SimpleBlog.MVC.Validation.ViewModels.Account;
using SimpleBlog.Services.Encryption;
using SimpleBlog.Services.Services.Abstract;

[assembly: InternalsVisibleTo("SimpleBlogTests")]

namespace SimpleBlog.MVC.Controllers
{
    public class SignUpController : Controller
    {
        private readonly ILogger<SignUpController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHandler _sessionHandler;
        private readonly string _signUpPagePath;
        private readonly IAccountService _accountService;
        private readonly SignUpModelValidator _accountValidator;

        public SignUpController(ILogger<SignUpController> logger,
            IConfiguration configuration, 
            IAccountService accountService,
            SignUpModelValidator accountValidator,
            ISessionHandler? sessionHandler = null)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHandler = sessionHandler ?? new SessionHandler();
            _signUpPagePath = "/Views/Authentication/SignUp.cshtml";
            _accountService = accountService;
            _accountValidator = accountValidator;
        }

        public IActionResult Index(CreatingAccountModel? model = null,
            ValidationResult? validationResult = null)
        {
            model ??= new();
            MainViewModel<CreatingAccountModel> mainViewModel = new(model, validationResult);
            return View(_signUpPagePath, mainViewModel);
        }

        [HttpPost]
        public IActionResult Register(CreatingAccountModel? model)
        {
            model ??= new();
            model.Password = Password.HashPassword(model.Password);
            ValidationResult? validationResult = _accountValidator.Validate(model);
            MainViewModel<CreatingAccountModel> mainViewModel = new(model, validationResult);
            
            if (!validationResult.IsValid)
                return View(_signUpPagePath, mainViewModel);

            IAccountService? accountService = _accountService;
            AccountDto accountDto = model.ParseToDto();
            accountService.AddModel(accountDto);
            _sessionHandler.SetOwnerId(accountDto.AccountId);
            return View(_signUpPagePath, mainViewModel);
        }
    }
}