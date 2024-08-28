using SimpleBlog.Common.DTOs;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories.Abstract;
using SimpleBlog.Services.Extensions;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.Services.Services;

public class PostLikeService : IPostLikeService
{
    private IPostLikeRepository _repository;

    public PostLikeService(IPostLikeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PostLikeDto>> GetAllModels()
    {
        List<PostLike> postLikes = await _repository.GetAll();
        return postLikes.ParseToModels();
    }

    public async Task<PostLikeDto?> GetModelById(int id)
    {
        PostLike? postLike = await _repository.GetById(id);
        return postLike?.ParseToDto();
    }

    public async Task<PostLikeDto> AddModel(PostLikeDto model)
    {
        if (await IsExist(model.ParseToEntity()))
            throw new ArgumentException("This like is exist");

        PostLike entity = model.ParseToEntity();
        await _repository.Add(entity);

        return entity.ParseToDto();
    }

    [Obsolete]
    public Task<PostLikeDto?> UpdateModel(PostLikeDto model)
    {
        throw new NotImplementedException();
    }

    public async Task<string> DeleteModel(int id)
    {
        PostLike? postLike = await _repository.GetById(id);

        if (postLike is null)
            throw new ArgumentException("This Like does not exist");

        await _repository.Delete(postLike);
        return "Deleted successfully";
    }
    
    public async Task<bool> IsExist(PostLike model)
    {
        List<PostLikeDto> allLikes = await GetAllModels();
        PostLikeDto? foundLike = allLikes
            .Find(postLikeDto => postLikeDto.LikeId == model.LikeId
                          && postLikeDto.PostId == model.PostId);

        return foundLike is not null;
    }

    public async Task<List<PostLikeDto>> GetAllPostLikeByPostId(int id)
    {
        List<PostLike> postLikes = await _repository.GetAllPostLikeByPostId(id);
        return postLikes.ParseToModels();
    }
}