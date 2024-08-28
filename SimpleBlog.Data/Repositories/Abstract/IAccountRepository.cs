using SimpleBlog.Data.Entities;

namespace SimpleBlog.Data.Repositories.Abstract;

public interface IAccountRepository : IRepository<Account>
{
    public Task<List<Account>> GetByNickname(string nickname);
    public Task<List<Account>> GetByEmail(string email);
}