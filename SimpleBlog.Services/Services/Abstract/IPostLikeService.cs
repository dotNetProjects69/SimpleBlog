using SimpleBlog.Common.DTOs;

namespace SimpleBlog.Services.Services.Abstract;

public interface IPostLikeService : IService<PostLikeDto>
{
    public Task<List<PostLikeDto>> GetAllPostLikeByPostId(int id);

}