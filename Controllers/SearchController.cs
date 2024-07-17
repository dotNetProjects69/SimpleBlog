using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;
using SimpleBlog.Shared;
using static SimpleBlog.Controllers.Extensions.Sql.AccountSql;

namespace SimpleBlog.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISessionHandler _sessionHandler;

        public SearchController(ISessionHandler? sessionHandler = null)
        {
            _sessionHandler = sessionHandler ?? new SessionHandler();
        }

        public IActionResult Index(Search? search = null)
        {
            search ??= new();
            return View(search);
        }

        public IActionResult Search(Search search)
        {
            search.Result.Clear();
            var result = 
                SelectAllFromTable($"WHERE NickName LIKE '%{search.Nickname}%'");
            foreach (IReadOnlyCollection<string> account in result)
            {
                string currentAccount = account.ElementAtOrDefault(7) ?? string.Empty;
                if (AccountIdIsNotCurrent(currentAccount))
                    search.Result.Add(currentAccount);
            }
            return RedirectToAction("Index", search);
        }

        public IActionResult ShowAllViewableAccountPosts(string nickname)
        {
            return RedirectToAction("Index", "AllPosts", new { nickname });
        }

        private bool AccountIdIsNotCurrent(string accountId)
        {
            return accountId != _sessionHandler.SessionOwnerId;
        }
    }
}
