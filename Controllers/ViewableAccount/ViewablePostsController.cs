using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models.Post;
using SimpleBlog.Models.ViewModels;
using static SimpleBlog.Controllers.Extensions.AccountSql;
using static SimpleBlog.Controllers.Extensions.PostSql;

namespace SimpleBlog.Controllers.ViewableAccount
{
    public class ViewablePostsController : Controller
    {
        private readonly string _allPostsViewPath = "/Views/Posts/ViewableAccount/AllPosts.cshtml";
        private readonly string _viewPostViewPath = "/Views/Posts/ViewableAccount/ViewPost.cshtml";

        public IActionResult Index(string nickname)
        {
            PostViewModel postListViewModel = new()
            {
                PostList = GetPosts("*", GetUserIdByNickname(nickname))
            };
            foreach (var postModel in postListViewModel.PostList)
            {
                postModel.Nickname = nickname;
            }
            return View(_allPostsViewPath, postListViewModel);
        }

        public IActionResult ViewPost(string nickname, int id)
        {
            PostModel post = GetPostById("*", GetUserIdByNickname(nickname), id).First();
            post.Nickname = nickname;
            PostViewModel postViewModel = new()
            {
                ViewablePost = post
            };
            return View(_viewPostViewPath, postViewModel);
        }

        private string GetUserIdByNickname(string nickname)
        {
            return SelectFromTableByWhere("UserID", "NickName", nickname)[0];
        }
    }
}
