using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Controllers.Extensions.Sql
{   
    public class TemplateBuilder
    {
        internal static string CreateTemplate(params string[] columnNames)
        {
            List<string> template = [];
            for (var i = 0; i < columnNames.Length; i++) 
                template.Insert(i, columnNames[i].Trim());

            StringBuilder stringBuilder = new();
            stringBuilder.AppendJoin(", ", template);
            return stringBuilder.ToString();
        }

        internal static string CreateValuesForm(params string[] columnValues)
        {
            List<string> values = [];
            for (var i = 0; i < columnValues.Length; i++)
                values.Insert(i, $"'{columnValues[i].Trim()}'");

            StringBuilder stringBuilder = new();
            stringBuilder.AppendJoin(", ", values);
            return stringBuilder.ToString();
        }
    }
}
