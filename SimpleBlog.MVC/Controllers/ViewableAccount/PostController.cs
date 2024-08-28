using Microsoft.AspNetCore.Mvc;
using ISessionHandler = SimpleBlog.MVC.Shared.ISessionHandler;
using SessionHandler = SimpleBlog.MVC.Shared.SessionHandler;

namespace SimpleBlog.MVC.Controllers.ViewableAccount
{
    public class PostController : Controller
    {
        private readonly string _viewPostViewPath = "/Views/Posts/ViewableAccount/ViewPost.cshtml";
        private readonly ISessionHandler _sessionHandler;

        public PostController(ISessionHandler? sessionHandler = null)
        {
            _sessionHandler = sessionHandler ?? new SessionHandler();
        }

        public IActionResult Index(string nickname, string postId)
        {
            throw new NotImplementedException();
        }

        public IActionResult LikeButtonClicked(string viewableNickname, string postId)
        {
            throw new NotImplementedException();
        }
    }
}
