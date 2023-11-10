using Microsoft.Data.Sqlite;
using SimpleBlog.Models.Account;
using System.Globalization;

namespace SimpleBlog.Controllers.Extensions
{
    public static class SqlExtensions
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
                                                                    .AddJsonFile("appsettings.json")
                                                                    .Build();

        internal static bool DropTable(string tableName, out string errorMessage)
        {
            errorMessage = string.Empty;
            string sqlCommand = $"DROP TABLE [{tableName}]";
            SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            SqliteCommand command = new(sqlCommand, connection);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }

        public static T InstantiateAccountModel<T>(string parameter, string value) where T : class, IAccountModel, new()
        {
            T model = new();
            string sqlCommand = $"SELECT * FROM AuthData WHERE {parameter} = '{value}'";
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            using SqliteCommand command = new(sqlCommand, connection);
            connection.Open();
            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows) return model;
                reader.Read();


                // use Visitor pattern
                if (model is EditAccountModel)
                    model = SetData(model as EditAccountModel ?? new(), reader) as T ?? new T();
                else
                    model = SetData(model, reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return model;
        }

        private static T SetData<T>(T model, SqliteDataReader reader) where T : IAccountModel
        {
            model.Id = new(reader.GetString(6));
            model.Name = reader.GetString(1);
            model.Surname = reader.GetString(2);
            _ = DateOnly.TryParse(reader.GetString(4),
                                  CultureInfo.CurrentCulture,
                                  DateTimeStyles.None,
                                  out DateOnly dateOnly);
            model.DateOfBirth = dateOnly;
            model.Email = reader.GetString(4);
            model.NickName = reader.GetString(7);
            return model;
        }

        private static EditAccountModel SetData(EditAccountModel model, SqliteDataReader reader)
        {
            model = SetData<EditAccountModel>(model, reader);
            model.Password = reader.GetString(5);
            return model;
        }
    }
}
