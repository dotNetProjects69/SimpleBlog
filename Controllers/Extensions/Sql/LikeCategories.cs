using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Controllers.Extensions.Sql
{
    public class LikeCategories
    {
        internal enum DbType
        {
            In,
            Out,
        }

        internal enum Action
        {
            Create,
            Insert,
        }
    }
}
