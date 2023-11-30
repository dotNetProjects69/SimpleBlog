using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Controllers.Extensions;
using SimpleBlog.Models;
using SimpleBlog.Models.ViewModels;
using static SimpleBlog.Models.TempData;

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
            _accountsData = _configuration.GetConnectionString("AccountsData") ?? "";
        }

        public IActionResult Index()
        {
            if (AccountTableName == string.Empty)
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
                Post = post
            };
            return View(postViewModel);
        }

        public IActionResult EditPost(int id)
        {
            PostModel post = GetPostById(id);
            PostViewModel postViewModel = new()
            {
                Post = post
            };
            return View(postViewModel);
        }

        private PostModel GetPostById(int id)
        {
            return PostSql.GetPostById("*", AccountTableName, id).First();
        }

        private PostViewModel GetAllPosts()
        {
            return new PostViewModel
            {
                PostList = PostSql.GetPosts("*", AccountTableName)
            };
        }

        public ActionResult Insert(PostModel post)
        {
            post.CreatedAt = DateTime.Now;
            post.UpdatedAt = DateTime.Now;


            using (SqliteConnection connection = new(_accountsData))
            {
                using SqliteCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = $"INSERT INTO [{AccountTableName}] " +
                    $"(Title, Body, CreatedAt, UpdatedAt) VALUES " +
                    $"('{post.Title}', '{post.Body}', '{post.CreatedAt.ToString(_format)}', '{post.UpdatedAt.ToString(_format)}')";
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

            using (SqliteConnection connection = new (_accountsData))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"UPDATE [{AccountTableName}] SET Title = " +
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
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"DELETE from [{AccountTableName}] WHERE Id = '{id}'";
                    command.ExecuteNonQuery();
                }
            }

            return Json(new object { });
        }

    }

}
