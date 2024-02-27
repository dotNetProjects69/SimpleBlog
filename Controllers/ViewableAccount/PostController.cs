using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models.Interfaces.PostModelParts;
using SimpleBlog.Models.ViewModels;
using SimpleBlog.Shared;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;
using static SimpleBlog.Controllers.Extensions.Sql.Likes;
using static SimpleBlog.Controllers.Extensions.Sql.PostSql;

namespace SimpleBlog.Controllers.ViewableAccount
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
            string accountId = GetAccountIdByNickname(nickname);
            IPostModel post = GetPostById("*", 
                                         accountId, 
                                         postId);
            post.Nickname = nickname;
            post.IsLiked = IsLikedBy(_sessionHandler.SessionOwnerId, post);
            post.Likes = GetNumberOfLikes(accountId, post.Id.ToString());
            PostViewModel postViewModel = new()
            {
                ViewablePost = post
            };
            return View(_viewPostViewPath, postViewModel);
        }

        public IActionResult LikeButtonClicked(string viewableNickname, string postId)
        {
            string accountId = _sessionHandler.SessionOwnerId;
            string viewableId = GetAccountIdByNickname(viewableNickname);
            IPostModel postModel = GetPostById("*", viewableId, postId);

            if (postModel.Error.StatusCodeIsNotOk())
                return RedirectToAction("Index", new { nickname = viewableNickname, postId });

            if (IsLikedBy(accountId, postModel))
                UnlikePost(viewableId, postId);
            else
                LikePost(viewableNickname, postId);

            return RedirectToAction("Index", new { nickname = viewableNickname, postId });
        }
    }
}
