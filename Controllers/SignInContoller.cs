using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.Controllers
{
    public class SignInContoller : Controller
    {

        private readonly ILogger<SignInContoller> _logger;
        private readonly IConfiguration _configuration;

        public SignInContoller(ILogger<SignInContoller> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        public IActionResult SignIn()
        {
            return View();
        }
    }
}
