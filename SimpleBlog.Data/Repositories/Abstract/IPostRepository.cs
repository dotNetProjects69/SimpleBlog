using SimpleBlog.Data.Entities;

namespace SimpleBlog.Data.Repositories.Abstract;

public interface IPostRepository : IRepository<Post>
{
    public Task<List<Post>> GetAllByAccountId(int id);
}