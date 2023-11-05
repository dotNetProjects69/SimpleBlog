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

        public IActionResult SignIn()
        {
            return View();
        }
    }
}
