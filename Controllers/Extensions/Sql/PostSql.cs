using Microsoft.Data.Sqlite;
using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces.PostModelParts;
using SimpleBlog.Models.Post;
using SimpleBlog.Shared;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;
using static SimpleBlog.Controllers.Extensions.Sql.LikeCategories.DbType;
using static SimpleBlog.Controllers.Extensions.Sql.Likes;
using static SimpleBlog.Shared.GlobalParams;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Controllers.Extensions.Sql
{
    public class PostSql
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
                                                                    .AddJsonFile("appsettings.json")
                                                                    .Build();
        private static string _format = _configuration["DateTimeFormat"] ?? "";
        private static SessionHandler _sessionHandler = new();
        private static string OwnerId => _sessionHandler.SessionOwnerId;

        

        internal static IPostModel GetPostById(string gettingParam,
                                               string accountTableName,
                                               string id)
        {
            return GetPosts(gettingParam, accountTableName, $"Where Id = '{id}'")
                .ElementAtOrDefault(0) ?? new PostModel()
            {
                Error = new ErrorModel(HttpStatusCode.BadRequest, "This post does not exist")
            };
        }

        internal static IReadOnlyCollection<IPostModel> GetPosts(string gettingParam, 
                                                                string accountTableName, 
                                                                string commandPostfix = "")
        {
            List<PostModel> postList = new();

            using SqliteConnection connection = new(GetAccountsDataPath());
            using SqliteCommand? command = connection.CreateCommand();
            connection.Open();
            command.CommandText = $"SELECT {gettingParam} FROM [{accountTableName}] {commandPostfix}";

            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);

                    DateTime.TryParseExact(reader.GetString(3),
                        _format,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime parsedCreatedAt);

                    DateTime.TryParseExact(reader.GetString(4),
                        _format,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime parsedUpdatedAt);
                    postList.Add(
                        new()
                        {
                            Id = id,
                            Title = reader.GetString(1),
                            Body = reader.GetString(2),
                            CreatedAt = parsedCreatedAt,
                            UpdatedAt = parsedUpdatedAt,
                            Nickname = GetAccountNicknameById(accountTableName),
                            Likes = GetNumberOfLikes(accountTableName, id.ToString())
                        });
                }
            }
            else
            {
                return postList;
            }

            ;

            return postList;
        }

        internal static void LikePost(string viewableNickname, string postId)
        {
            string viewableId = GetAccountIdByNickname(viewableNickname);;
            string accountId = _sessionHandler.SessionOwnerId;
            IPostModel post = GetPostById("*", viewableId, postId);
            bool postLiked = IsLikedBy(_sessionHandler.SessionOwnerId, post);

            if (!postLiked && 
                !string.IsNullOrEmpty(accountId) && 
                !string.IsNullOrEmpty(viewableId))
            {
                ProcessLikes(viewableId, postId, accountId, In);
                ProcessLikes(accountId, viewableId, postId, Out);
            }
        }

        private static void ProcessLikes(string fileName,
            string tableName,
            string tableFirstValue,
            LikeCategories.DbType directory)
        {
            Likes likes = new(directory)
            {
                Directory = fileName
            };
            likes.CreateDbFileIfNecessary();
            likes.CreateTableIfNecessary(tableName);
            likes.InsertDataToTable(tableName, tableFirstValue, DateTime.Now);
        }

        public static void UnlikePost(string viewableId, string postId)
        {
            string accountId = _sessionHandler.SessionOwnerId;

            if (accountId == string.Empty || viewableId == string.Empty)
            {
                Console.WriteLine("Nickname to Id convert error");
                return;
            }

            string dbInFilePath = GetRelativeDbFilePath(viewableId, In);
            DeleteAccount(postId, accountId, dbInFilePath);

            
            string dbOutFilePath = GetRelativeDbFilePath(accountId, Out);
            DeletePost(viewableId, postId, dbOutFilePath);

        }

        internal static bool IsLikedBy(string accountId, IPostModel model)
        {
            string modelUserId = GetAccountIdByNickname(model.Nickname);
            return Likes.IsLikedBy(accountId, modelUserId, model.Id.ToString());
        }
    }
}
