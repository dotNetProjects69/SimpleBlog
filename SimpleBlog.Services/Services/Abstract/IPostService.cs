using SimpleBlog.Common.DTOs;

namespace SimpleBlog.Services.Services.Abstract;

public interface IPostService : IService<PostDto>
{
    public Task<List<PostDto>> GetPostsByAccountId(int id);

}