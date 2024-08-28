using Microsoft.AspNetCore.Mvc.Rendering;

namespace SimpleBlog.MVC.Views
{
    public static class Extensions
    {
        public static bool IsDebug(this IHtmlHelper htmlHelper)
        {
#if DEBUG
            return true;
#else
      return false;
#endif
        }
    }
}
