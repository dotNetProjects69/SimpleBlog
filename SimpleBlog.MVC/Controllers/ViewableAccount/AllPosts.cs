using Microsoft.AspNetCore.Mvc;
using ISessionHandler = SimpleBlog.MVC.Shared.ISessionHandler;

namespace SimpleBlog.MVC.Controllers.ViewableAccount
{
    public class AllPosts : Controller
    {
        private readonly ISessionHandler _sessionHandler;
        private readonly string _allPostsViewPath = "/Views/Posts/ViewableAccount/AllPosts.cshtml";

        public AllPosts(ISessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }

        public IActionResult Index(string nickname)
        {
            throw new NotImplementedException();
        }

        public IActionResult ViewPost(string nickname, int id)
        {
            return RedirectToAction("Index", "Post", new { nickname, postId = id });
        }
        
        public IActionResult LikeButtonClicked(string viewableNickname, string postId)
        {
            throw new NotImplementedException();
        }
    }
}
