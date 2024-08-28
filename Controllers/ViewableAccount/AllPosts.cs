using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SimpleBlog.Models.Interfaces.PostModelParts;
using SimpleBlog.Models.ViewModels;
using SimpleBlog.Shared;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;
using static SimpleBlog.Controllers.Extensions.Sql.PostSql;

namespace SimpleBlog.Controllers.ViewableAccount
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
            string viewableId = GetAccountIdByNickname(nickname);

            PostViewModel postListViewModel = new()
            {
                PostList = GetPosts("*", viewableId)
            };

            foreach (IPostModel? postModel in postListViewModel.PostList)
            {
                postModel.IsLiked = IsLikedBy(_sessionHandler.SessionOwnerId, postModel);
            }
            return View(_allPostsViewPath, postListViewModel);
        }

        public IActionResult ViewPost(string nickname, int id)
        {
            return RedirectToAction("Index", "Post", new { nickname, postId = id });
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
