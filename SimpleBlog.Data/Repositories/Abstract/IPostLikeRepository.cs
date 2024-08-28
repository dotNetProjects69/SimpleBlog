using SimpleBlog.Data.Entities;

namespace SimpleBlog.Data.Repositories.Abstract;

public interface IPostLikeRepository : IRepository<PostLike>
{
    Task<List<PostLike>> GetAllPostLikeByPostId(int id);
}