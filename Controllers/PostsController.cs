using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models.ViewModels;
using static SimpleBlog.Models.TempData;
using static SimpleBlog.Shared.GlobalParams;
using static SimpleBlog.Controllers.Extensions.PostSql;
using static SimpleBlog.Controllers.Extensions.AccountSql;
using SimpleBlog.Models.Post;

namespace SimpleBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _format;
        private readonly string _accountsData;

        public PostsController(ILogger<PostsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _format = _configuration["DateTimeFormat"] ?? "";
            _accountsData = GetAccountsDataPath();
        }

        public IActionResult Index()
        {
            if (GetCurrentAccountId() == string.Empty)
                return RedirectToAction("Index", "SignUp");
            PostViewModel postListViewModel = GetAllPosts();
            return View(postListViewModel);
        }

        public IActionResult NewPost()
        {
            return View();
        }

        public IActionResult ViewPost(int id)
        {
            PostModel post = GetPostById(id);
            PostViewModel postViewModel = new()
            {
                ViewablePost = post
            };
            return View(postViewModel);
        }

        public IActionResult EditPost(int id)
        {
            PostModel post = GetPostById(id);
            PostViewModel postViewModel = new()
            {
                ViewablePost = post
            };
            return View(postViewModel);
        }

        private PostModel GetPostById(int id)
        {
            return PostSql.GetPostById("*", GetCurrentAccountId(), id).First();
        }

        private PostViewModel GetAllPosts()
        {
            return new PostViewModel
            {
                PostList = GetPosts("*", GetCurrentAccountId())
            };
        }

        public ActionResult Insert(PostModel viewablePost)
        {
            viewablePost.CreatedAt = DateTime.Now;
            viewablePost.UpdatedAt = DateTime.Now;


            using (SqliteConnection connection = new(_accountsData))
            {
                using SqliteCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = $"INSERT INTO [{GetCurrentAccountId()}] " +
                    $"(Title, Body, CreatedAt, UpdatedAt) VALUES " +
                    $"('{viewablePost.Title}', '{viewablePost.Body}', '{viewablePost.CreatedAt.ToString(_format)}', '{viewablePost.UpdatedAt.ToString(_format)}')";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(PostModel post)
        {
            post.UpdatedAt = DateTime.Now;

            using (SqliteConnection connection = new(_accountsData))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"UPDATE [{GetCurrentAccountId()}] SET Title = " +
                                          $"'{post.Title}', " +
                                          $"Body = '{post.Body}', " +
                                          $"UpdatedAt = '{post.UpdatedAt}' " +
                                          $"WHERE Id = '{post.Id}'";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (SqliteConnection connection = new (_accountsData))
            {
                using var command = connection.CreateCommand();
                connection.Open();
                command.CommandText = $"DELETE from [{GetCurrentAccountId()}] WHERE Id = '{id}'";
                command.ExecuteNonQuery();
            }

            return Json(new object { });
        }

        private string GetCurrentAccountId()
        {
            return HttpContext.Session.GetString(AccountIdSessionKey) ?? "";
        }
    }

}
