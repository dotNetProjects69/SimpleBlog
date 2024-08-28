using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Common.DTOs;
using SimpleBlog.Data.Entities;
using SimpleBlog.MVC.Shared;
using SimpleBlog.MVC.Validation.ViewModels.Authentication;
using SimpleBlog.Services.Services;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.MVC.Controllers.SignInControllers
{
    public abstract class SignInController : Controller
    {
        private readonly ILogger<SignInController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHandler _sessionHandler;
        private protected SignInModel? Model;
        private protected readonly string SignInByNicknamePagePath;
        private protected readonly string SignInByEmailPagePath;
        private protected readonly IAccountService AccountService;

        protected SignInController(ILogger<SignInController> logger,
            IConfiguration configuration, 
            IAccountService accountService,
            ISessionHandler? sessionHandler = null)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHandler = sessionHandler ?? new SessionHandler();
            AccountService = accountService;
            SignInByNicknamePagePath = "/Views/Authentication/SignInByNickname.cshtml";
            SignInByEmailPagePath = "/Views/Authentication/SignInByEmail.cshtml";
        }

        public abstract IActionResult Index(SignInModel? model = null);

        private protected void SaveAccountIdToSession(int id)
        {
            _sessionHandler.SetOwnerId(id);
        }
    }
}