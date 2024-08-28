using Microsoft.AspNetCore.Mvc;
using SimpleBlog.MVC.Validation.ViewModels;
using ISessionHandler = SimpleBlog.MVC.Shared.ISessionHandler;
using SessionHandler = SimpleBlog.MVC.Shared.SessionHandler;

namespace SimpleBlog.MVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISessionHandler _sessionHandler;

        public SearchController(ISessionHandler? sessionHandler = null)
        {
            _sessionHandler = sessionHandler ?? new SessionHandler();
        }

        public IActionResult Index(Search? search = null)
        {
            throw new NotImplementedException();
        }

        public IActionResult Search(Search search)
        {
            throw new NotImplementedException();
        }

        public IActionResult ShowAllViewableAccountPosts(string nickname)
        {
            throw new NotImplementedException();
        }
    }
}
