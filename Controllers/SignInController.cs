using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;

namespace SimpleBlog.Controllers
{
    public class SignInController : Controller
    {
        private readonly ILogger<SignInController> _logger;
        private readonly IConfiguration _configuration;

        public SignInController(ILogger<SignInController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        public IActionResult ShowSignUpPage()
        {
            AccountModel model = new();
            return View("SignUp", model);
        }
    }
}
