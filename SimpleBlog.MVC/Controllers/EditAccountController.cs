using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Common.DTOs;
using SimpleBlog.MVC.Extensions;
using SimpleBlog.MVC.Models;
using SimpleBlog.MVC.Shared;
using SimpleBlog.MVC.Validation.ValidationV2.ViewModel;
using SimpleBlog.MVC.Validation.ViewModels;
using SimpleBlog.MVC.Validation.ViewModels.Account;
using SimpleBlog.Services.Encryption;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.MVC.Controllers
{
    public class EditAccountController : Controller
    {
        private readonly ILogger<EditAccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _editAccountPagePath;
        private readonly ISessionHandler _sessionHandler;
        private readonly IAccountService _accountService;
        private readonly UpdatingAccountModelValidator _accountValidator;

        public EditAccountController(
            ILogger<EditAccountController> logger,
            IConfiguration configuration,
            IAccountService accountService,
            UpdatingAccountModelValidator accountValidator)
        {
            _logger = logger;
            _configuration = configuration;
            _accountService = accountService;
            _accountValidator = accountValidator;
            _editAccountPagePath = "/Views/PersonalAccount/EditAccount.cshtml";
            _sessionHandler = new SessionHandler();
        }
        
        public async Task<IActionResult> Index()
        {
            int accountId = _sessionHandler.GetOwnerId();
            AccountDto? accountDto = await _accountService.GetModelById(accountId);

            if (accountDto is null)
                throw new($"Account with id - {accountDto} not found");

            UpdatingAccountModel updatingAccount = accountDto.ParseToViewModel();
            updatingAccount.Password = string.Empty;
            MainViewModel<UpdatingAccountModel> mainViewModel = new(updatingAccount);
            return View(_editAccountPagePath, mainViewModel);
        }

        public IActionResult Update(UpdatingAccountModel? model)
        {
            AccountDto? accountDto = model?.ParseToDto();

            if (accountDto is null || model is null)
                return RedirectToAction("Index", "PersonalAccount");
            // todo add notification "Error while saving changes"

            accountDto.Password = Password.HashPassword(model.Password);
            ValidationResult? validationResult = _accountValidator.Validate(model);
            UpdatingAccountModel updatingAccount = accountDto.ParseToViewModel();
            updatingAccount.Password = string.Empty;
            MainViewModel<UpdatingAccountModel> mainViewModel = new(updatingAccount, validationResult);

            if (!validationResult.IsValid)
                return View(_editAccountPagePath, mainViewModel);

            _accountService.UpdateModel(accountDto);

            return RedirectToAction("Index", "PersonalAccount");
        }
    }
}