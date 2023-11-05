using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Models;
using SimpleBlog.Models.ViewModels;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace SimpleBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IConfiguration _configuration;

        public PostsController(ILogger<PostsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        public IActionResult Index()
        {
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
            Console.WriteLine(post.Title);
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
            PostModel post = new();

            using (SqliteConnection connection = new(_configuration.GetConnectionString("BlogDataContext")))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"SELECT * FROM post Where Id = '{id}'";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                DateTimeFormatInfo dateTimeFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
                                string dateFormat = dateTimeFormatInfo.ShortDatePattern;
                                string timeFormat = dateTimeFormatInfo.LongTimePattern;

                                string format = $"{dateFormat} H{timeFormat}";
                                Console.WriteLine(format);
                                DateTime.TryParseExact(reader.GetString(3),
                                                       format,
                                                       CultureInfo.InvariantCulture,
                                                       DateTimeStyles.None,
                                                       out var parsedCreatedAt);

                                DateTime.TryParseExact(reader.GetString(3),
                                                       format,
                                                       CultureInfo.InvariantCulture,
                                                       DateTimeStyles.None,
                                                       out var parsedUpdatedAt);
                                post.Id = reader.GetInt32(0);
                                post.Title = reader.GetString(1);
                                post.Body = reader.GetString(2);
                                post.CreatedAt = reader.GetDateTime(3);
                                post.UpdatedAt = reader.GetDateTime(4);

                                return post;
                            }
                        }
                        else
                            return post;
                    };
                }
            }

            return post;
        }

        public PostViewModel GetAllPosts()
        {
            List<PostModel> postList = new();

            using (SqliteConnection connection = new(_configuration.GetConnectionString("BlogDataContext")))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "SELECT * FROM post";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine($"reader has rows");
                            while (reader.Read())
                            {
                                DateTimeFormatInfo dateTimeFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
                                string dateFormat = dateTimeFormatInfo.ShortDatePattern;
                                string timeFormat = dateTimeFormatInfo.LongTimePattern;

                                string format = $"{dateFormat} H{timeFormat}";
                                DateTime.TryParseExact(reader.GetString(3),
                                                       format, 
                                                       CultureInfo.InvariantCulture,
                                                       DateTimeStyles.None,
                                                       out var parsedCreatedAt);

                                DateTime.TryParseExact(reader.GetString(3),
                                                       format,
                                                       CultureInfo.InvariantCulture,
                                                       DateTimeStyles.None,
                                                       out var parsedUpdatedAt);
                                postList.Add(
                                    new PostModel
                                    {
                                        Id = reader.GetInt32(0),
                                        Title = reader.GetString(1),
                                        Body = reader.GetString(2),
                                        CreatedAt = parsedCreatedAt,
                                        UpdatedAt = parsedUpdatedAt
                                    });
                            }
                        }
                        else
                        {
                            return new PostViewModel
                            {
                                PostList = postList
                            };
                        }
                    };
                }
            }

            return new PostViewModel
            {
                PostList = postList
            };
        }

        public ActionResult Insert(PostModel post)
        {
            post.CreatedAt = DateTime.Now;
            post.UpdatedAt = DateTime.Now;

            using (SqliteConnection connection = new(_configuration.GetConnectionString("BlogDataContext")))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO post (Title, Body, CreatedAt, UpdatedAt) VALUES ('{post.Title}', '{post.Body}', '{post.CreatedAt}', '{post.UpdatedAt}')";
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

        public ActionResult Update(PostModel post)
        {
            post.UpdatedAt = DateTime.Now;

            using (SqliteConnection connection = new (_configuration.GetConnectionString("BlogDataContext")))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"UPDATE post SET Title = " +
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
            using (SqliteConnection connection =
                new (_configuration.GetConnectionString("BlogDataContext")))
            {
                using (var command = connection.CreateCommand())
                {
                    Console.WriteLine("Trying to delete");
                    connection.Open();
                    command.CommandText = $"DELETE from post WHERE Id = '{id}'";
                    command.ExecuteNonQuery();
                }
            }

            return Json(new object { });
        }

    }

}
