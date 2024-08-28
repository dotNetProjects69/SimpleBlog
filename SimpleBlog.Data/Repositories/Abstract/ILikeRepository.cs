using SimpleBlog.Data.Entities;

namespace SimpleBlog.Data.Repositories.Abstract;

public interface ILikeRepository : IRepository<Like>
{
    Task<List<Like>> GetAllLikesByAccountId(int id);
}