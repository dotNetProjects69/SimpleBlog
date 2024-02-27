using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;
using static SimpleBlog.Controllers.Extensions.Sql.LikeCategories.Action;
using static SimpleBlog.Shared.GlobalParams;
using static System.Console;
using LikeDirectory = System.Collections.Generic.Dictionary
<SimpleBlog.Controllers.Extensions.Sql.LikeCategories.DbType,
    System.Collections.Generic.Dictionary
    <SimpleBlog.Controllers.Extensions.Sql.LikeCategories.Action,
        string>>;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Controllers.Extensions.Sql
{
    public class Likes
    {
        private string _dbAbsoluteFilePath = string.Empty;
        private string _dbRelativeFilePath = string.Empty;
        private string _tableName = string.Empty;
        private LikeCategories.DbType _dbType;
        private LikeDirectory _templates;

        internal string Directory
        {
            set
            {
                _dbAbsoluteFilePath = GetAbsoluteDbFilePath(value, _dbType);
                _dbRelativeFilePath = GetRelativeDbFilePath(value, _dbType);
            }
        }

        private Likes()
        {
            _templates = InstantiateTemplates();
        }

        private LikeDirectory InstantiateTemplates()
        {
            return new()
            {
                {
                    LikeCategories.DbType.In, new()
                    {
                        { Create, "(Id integer primary key, AccountID text, Time text)" },
                        { Insert, "(AccountID, Time)" }
                    }
                },
                {
                    LikeCategories.DbType.Out, new()
                    {
                        { Create, "(Id integer primary key, PostID text, Time text)"},
                        { Insert,  "(PostID, Time)"}
                    }
                }
            };
        }

        internal Likes(LikeCategories.DbType dbType) : this()
        {
            _dbType = dbType;
        }

        internal static void DeleteAccount(string tableName, 
                                           string selectableValue, 
                                           string dbFilePath)
        {
            DeleteRow(tableName, "AccountID", selectableValue, dbFilePath);
        }

        internal static void DeletePost(string tableName, 
                                        string selectableValue, 
                                        string dbFilePath)
        {
            DeleteRow(tableName, "PostID", selectableValue, dbFilePath);
        }
        
        private static void DeleteRow(string tableName, 
                                      string filterName, 
                                      string filterParam, 
                                      string directoryPath)
        {
            if (!ParamExist(tableName, filterName, filterParam, directoryPath))
                return;
            string sqlCommand = $"DELETE FROM [{tableName}] WHERE {filterName} = '{filterParam}'";
            SqliteConnection connection = new($"Data Source={directoryPath}");
            SqliteCommand command = new(sqlCommand, connection);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        private static bool ParamExist(string tableName, 
                                       string filterName, 
                                       string filterParam, 
                                       string directoryPath)
        {
            using SqliteConnection connection = new($"Data Source={directoryPath}");
            connection.Open();

            string selectCommand = $"SELECT EXISTS(SELECT 1 FROM '{tableName}' WHERE {filterName} = '{filterParam}')";

            using var command = new SqliteCommand(selectCommand, connection);
            long res = (long)(command.ExecuteScalar() ?? 0);

            return res > 0;
        }

        internal static string GetRelativeDbFilePath(string accountId, LikeCategories.DbType directory)
        {
            return Path.Combine(GetRelativeDbDirectoryPath(),
                "Like",
                directory.ToString(),
                $"{accountId}.sqlite");
        }

        internal static int GetNumberOfLikes(string accountId, string postId)
        {
            if (!File.Exists(GetAbsoluteDbFilePath(accountId, LikeCategories.DbType.In)))
                return 0;

            using SqliteConnection connection = 
                new($"Data Source={GetRelativeDbFilePath(accountId, 
                    LikeCategories.DbType.In)}");
            connection.Open();

            string command = $"SELECT COUNT(*) FROM '{postId}'";
            using SqliteCommand sqliteCommand = new(command, connection);
            long res = (long)(sqliteCommand.ExecuteScalar() ?? 0);

            return (int)res;
        }

        internal static bool IsLikedBy(string accountId, string viewableId, string postId)
        {
            if (!File.Exists(GetAbsoluteDbFilePath(viewableId, LikeCategories.DbType.In)))
                return false;

            using SqliteConnection connection = 
                new($"Data Source={GetRelativeDbFilePath(viewableId, 
                    LikeCategories.DbType.In)}");
            connection.Open();

            string command = $"SELECT COUNT('{accountId}') FROM '{postId}'";
            using SqliteCommand sqliteCommand = new(command, connection);
            long res = (long)(sqliteCommand.ExecuteScalar() ?? 0);

            return res > 0;
        }

        internal static string GetAbsoluteDbFilePath(string accountId, LikeCategories.DbType directory)
        {
            return Path.Combine(GetAbsoluteDbDirectoryPath(),
                "Like",
                directory.ToString(),
                $"{accountId}.sqlite");
        }

        internal void CreateDbFileIfNecessary()
        {

            if(!File.Exists(_dbAbsoluteFilePath)) 
                File.Create(_dbAbsoluteFilePath).Dispose();
        }

        internal void CreateTableIfNecessary(string tableName)
        {
            string dbFilePath = $"Data Source={_dbRelativeFilePath}";
            _tableName = tableName;
            if(!TableExist(dbFilePath))
                CreateTable(tableName, _templates[_dbType][Create], dbFilePath);
        }

        private bool TableExist(string dbFilePath)
        {
            using SqliteConnection connection = new(dbFilePath);
            using var command = connection.CreateCommand();
            {
                connection.Open();

                command.CommandText = $"SELECT name FROM sqlite_master " +
                                      $"WHERE type='table' AND name='{_tableName}'";
                try
                {
                    return command.ExecuteReader().HasRows;
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
            return false;
        }

        internal void InsertDataToTable(string tableName, string firstParam, DateTime dateTime)
        {
            using SqliteConnection connection = new($"Data Source={_dbRelativeFilePath}");
            using var command = connection.CreateCommand();
            {
                connection.Open();
                command.CommandText = $"INSERT INTO [{tableName}] {_templates[_dbType][Insert]} " +
                                      $"VALUES ('{firstParam}', '{dateTime}')";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
        }
    }
}
