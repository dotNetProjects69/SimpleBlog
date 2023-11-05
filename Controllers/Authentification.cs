using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.Controllers
{
    public class Authentification : Controller
    {
        private readonly ILogger<Authentification> _logger;
        private readonly IConfiguration _configuration;

        public Authentification(ILogger<Authentification> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Register()
        {
            // In future we must go to posts for this account
            return RedirectToAction("Index", "Home");
        }
    }
}
