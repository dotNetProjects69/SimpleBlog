using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SimpleBlog.Models;
using SimpleBlog.Models.ViewModels;
using System.Globalization;

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

        internal PostViewModel GetAllPosts()
        {
            List<PostModel> postList = new();

            using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
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

            using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
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
    }
}
