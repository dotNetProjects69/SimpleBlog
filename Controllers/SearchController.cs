using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;
using SimpleBlog.Models.ViewModels;
using static SimpleBlog.Controllers.Extensions.AccountSql;
using static SimpleBlog.Controllers.Extensions.PostSql;

namespace SimpleBlog.Controllers
{
    public class SearchController : Controller
    {

        public IActionResult Index(Search? search = null)
        {
            search ??= new();
            return View(search);
        }

        public IActionResult Search(Search search)
        {
            search.Result.Clear();
            var result = SelectAllFromTable($"WHERE NickName LIKE '%{search.Nickname}%'");
            foreach (var account in result)
                if (account[7] != Models.TempData.AccountTableName)
                    search.Result.Add(account[7]);
            return RedirectToAction("Index", search);
        }

        public IActionResult ShowAllViewableAccountPosts(string nickname)
        {
            return RedirectToAction("Index", "ViewablePosts", new { nickname });
        }
    }
}
