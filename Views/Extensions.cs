using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SimpleBlog.Views
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
