using Microsoft.Data.Sqlite;
using SimpleBlog.Models.Account;
using System.Globalization;
using System.Runtime.CompilerServices;
using static System.Console;
using static SimpleBlog.Shared.GlobalParams;

[assembly: InternalsVisibleTo("SimpleBlogTests")]

namespace SimpleBlog.Controllers.Extensions
{
    public class AccountSql
    {

        internal static void DropTable(string tableName)
        {
            string sqlCommand = $"DROP TABLE [{tableName}]";
            SqliteConnection connection = new(GetAccountsDataPath());
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
        
        internal static T InstantiateAccountModelOrEmpty<T>(string parameter, string value) where T : class, IAccount, new()
        {
            T model = new();
            IReadOnlyList<string> data = SelectFromTableByWhere("*", parameter, value);
            if (!data.Any()) return model;

            // use Visitor pattern
            if (model is EditAccountModel editAccountModel)
                model = SetData(editAccountModel, data) as T ?? new T();
            else
                model = SetData(model, data);
            return model;
        }

        internal static IReadOnlyList<string> SelectFromTableByWhere(string selectable, string filterParam,
                                                                     string filterName, string tableName = "AuthData")
        {
            string filter = $"WHERE {filterParam} = '{filterName}'";
            if(SelectFromTable(selectable, filter, tableName).Any()) 
                return SelectFromTable(selectable, filter, tableName).First();
            return new List<string>();
        }

        internal static IReadOnlyList<IReadOnlyList<string>> SelectAllFromTable(string filter = "", string tableName = "AuthData")
        {
            return SelectFromTable("*", filter, tableName);
        }

        internal static IReadOnlyList<IReadOnlyList<string>> SelectFromTable(string selectable, string filter = "",
                                                                             string tableName = "AuthData")
        {
            List<List<string>> accounts = new();
            string sqlCommand = $"SELECT {selectable} FROM {tableName} {filter}";
            using SqliteConnection connection = new(GetAccountsDataPath());
            using SqliteCommand command = new(sqlCommand, connection);
            connection.Open();
            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    List<string> account = new();
                    for (var i = 0; i < reader.FieldCount; i++) 
                        account.Add(reader.GetString(i));
                    accounts.Add(account);
                }
                return accounts;
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }

            return accounts;
        }

        private static T SetData<T>(T model, IEnumerable<string> data) where T : IAccount
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
    }
}
