using Microsoft.Data.Sqlite;
using SimpleBlog.Models.Account;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleBlogTest")]

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

        #region by data list
        public static T InstantiateAccountModel<T>(string parameter, string value) where T : class, IAccountModel, new()
        {
            T model = new();
            IEnumerable<string> data = SelectFromTable("*", parameter, value);
            if (data.Count() == 0) return model;

            // use Visitor pattern
            if (model is EditAccountModel)
                model = SetData(model as EditAccountModel ?? new(), data) as T ?? new T();
            else
                model = SetData(model, data);
            return model;
        }

        internal static IEnumerable<string> SelectFromTable(string selectable,
                                                            string filterParam,
                                                            string filterName,
                                                            string tableName = "AuthData")
        {
            List<string> data = new ();
            string sqlCommand = $"SELECT {selectable} FROM {tableName} WHERE {filterParam} = '{filterName}'";
            using SqliteConnection connection = new(_configuration.GetConnectionString("AccountsData"));
            using SqliteCommand command = new(sqlCommand, connection);
            connection.Open();
            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows && reader is not null)
                {
                    if (selectable == "*")
                    for (int i = 0; i < 8; i++)
                            data.Add(reader.GetString(i));
                    else
                        data.Add(reader.GetString(0));
                } 
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return data;
        }

        private static T SetData<T>(T model, IEnumerable<string> data) where T : IAccountModel
        {
            model.Id = new(data.ElementAt(6));
            model.Name = data.ElementAt(1);
            model.Surname = data.ElementAt(2);
            _ = DateOnly.TryParse(data.ElementAt(3),
                                  CultureInfo.CurrentCulture,
                                  DateTimeStyles.None,
                                  out DateOnly dateOnly);
            model.DateOfBirth = dateOnly;
            model.Email = data.ElementAt(4);
            model.NickName = data.ElementAt(7);
            return model;
        }

        private static EditAccountModel SetData(EditAccountModel model, IEnumerable<string> data)
        {
            model = SetData<EditAccountModel>(model, data);
            model.Password = data.ElementAt(5);
            return model;
        }
        #endregion
    }
}
