using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;
using static SimpleBlog.Controllers.Extensions.AccountSql;
using static SimpleBlog.Models.TempData;

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
                if (AccountIdIsNotCurrent(account[7]))
                    search.Result.Add(account[7]);
            return RedirectToAction("Index", search);
        }

        public IActionResult ShowAllViewableAccountPosts(string nickname)
        {
            return RedirectToAction("Index", "ViewablePosts", new { nickname });
        }

        private bool AccountIdIsNotCurrent(string accountId)
        {
            return accountId != HttpContext.Session.GetString(AccountIdSessionKey);
        }
    }
}
