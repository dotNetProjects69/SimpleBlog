using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.Controllers
{
    public class SignUpContoller : Controller
    {

        private readonly ILogger<SignUpContoller> _logger;
        private readonly IConfiguration _configuration;

        public SignUpContoller(ILogger<SignUpContoller> logger, IConfiguration configuration)
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
