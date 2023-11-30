using Microsoft.Data.Sqlite;
using SimpleBlog.Models;
using System.Globalization;

namespace SimpleBlog.Controllers.Extensions
{
    public class PostSql
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
                                                                    .AddJsonFile("appsettings.json")
                                                                    .Build();
        private static string _format = _configuration["DateTimeFormat"] ?? "";

        internal static List<PostModel> GetPostById(string gettingParam, string accountTableName, int id)
        {
            return GetPosts(gettingParam, accountTableName, $"Where Id = '{id}'");
        }

        internal static List<PostModel> GetPosts(string gettingParam, string accountTableName, string commandPostfix = "")
        {
            List<PostModel> postList = new();

            using (SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData")))
            {
                using var command = connection.CreateCommand();
                connection.Open();
                command.CommandText = $"SELECT {gettingParam} FROM [{accountTableName}] {commandPostfix}";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime.TryParseExact(reader.GetString(3),
                                                   _format,
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None,
                                                   out var parsedCreatedAt);

                            DateTime.TryParseExact(reader.GetString(4),
                                                   _format,
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
                        return postList;
                    }
                };
            }

            return postList;
        }
    }
}
