using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models.ViewModels;
using static SimpleBlog.Models.TempData;
using static SimpleBlog.Shared.GlobalParams;
using static SimpleBlog.Controllers.Extensions.PostSql;
using SimpleBlog.Models.Post;

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
                PostList = GetPosts("*", nickname)
            };
            foreach (var postModel in postListViewModel.PostList)
            {
                postModel.Nickname = nickname;
            }
            return View(_allPostsViewPath,postListViewModel);
        }

        public IActionResult ViewPost(string nickname, int id)
        {
            PostModel post = GetPostById("*", nickname, id).First();
            post.Nickname = nickname;
            PostViewModel postViewModel = new()
            {
                ViewablePost = post
            };
            return View(_viewPostViewPath, postViewModel);
        }
    }
}
