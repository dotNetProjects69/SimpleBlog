using SimpleBlog.Common.DTOs;

namespace SimpleBlog.Services.Services.Abstract;

public interface ILikeService : IService<LikeDto>
{
    public Task<List<LikeDto>> GetAllLikesByAccountOwnerId(int id);
}