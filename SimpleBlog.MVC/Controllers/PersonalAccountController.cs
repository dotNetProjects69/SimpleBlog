using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Common.DTOs;
using SimpleBlog.MVC.Extensions;
using SimpleBlog.MVC.Shared;
using SimpleBlog.MVC.Validation.ViewModels.Account;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.MVC.Controllers
{
    public class PersonalAccountController : Controller
    {
        private readonly ILogger<PersonalAccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHandler _sessionHandler;
        private readonly IAccountService _accountService;
        private readonly IPostService _postService;

        public PersonalAccountController(ILogger<PersonalAccountController> logger, 
            IConfiguration configuration,
            IAccountService accountService, 
            IPostService postService, 
            ISessionHandler sessionHandler)
        {
            _logger = logger;
            _configuration = configuration;
            _accountService = accountService;
            _postService = postService;
            _sessionHandler = sessionHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int accountId = _sessionHandler.GetOwnerId();
            AccountDto? account = await _accountService.GetModelById(accountId);
            UpdatingAccountModel? updatingAccount = account?.ParseToViewModel();
            return View(updatingAccount);
        }


        public async Task<IActionResult> Delete()
        {
            int accountId = _sessionHandler.GetOwnerId();
            await _accountService.DeleteModel(accountId);
            _sessionHandler.SetUnauthorizedId();
            await DeleteAllPostsByAccountId(accountId);
            // todo delete all post-like data
            // todo delete all likes data
            return RedirectToAction("Index", "SignInByEmail");
        }

        private async Task DeleteAllPostsByAccountId(int accountId)
        {
            List<PostDto> posts = await _postService.GetPostsByAccountId(accountId);
            foreach (PostDto post in posts)
            {
                await _postService.DeleteModel(post.PostId);
            }
        }


        public IActionResult LogOut()
        {
            _sessionHandler.SetUnauthorizedId();
            return RedirectToAction("Index", "SignInByEmail");
        }


        public IActionResult Edit()
        {
            return RedirectToAction("Index", "EditAccount");
        }
    }
}