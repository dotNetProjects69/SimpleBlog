using SimpleBlog.Common.DTOs;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories.Abstract;
using SimpleBlog.Services.Extensions;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.Services.Services;

public class LikeService : ILikeService
{
    private ILikeRepository _repository;

    public LikeService(ILikeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<LikeDto>> GetAllModels()
    {
        List<Like> likes = await _repository.GetAll();
        return likes.ParseToModels();
    }

    public async Task<LikeDto?> GetModelById(int id)
    {
        Like? like = await _repository.GetById(id);
        return like is not null
            ? like.ParseToDto()
            : throw new ArgumentException("This like not exist");
    }
    
    

    public async Task<LikeDto> AddModel(LikeDto model)
    {
        if (await IsExist(model.ParseToEntity()))
            throw new ArgumentException("This like is exist");

        Like entity = model.ParseToEntity();
        await _repository.Add(entity);

        return entity.ParseToDto();
    }

    /// <summary>
    /// Don't use this method, dur to like can't be changed, only added and removed
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [Obsolete("Don't use this method, dur to like can't be changed, only added and removed")]
    public Task<LikeDto?> UpdateModel(LikeDto model)
    {
        throw new NotImplementedException();
    }

    public async Task<string> DeleteModel(int id)
    {
        Like? like = await _repository.GetById(id);

        if (like is null)
            throw new ArgumentException("This Like does not exist");

        await _repository.Delete(like);
        return "Deleted successfully";
    }

    public async Task<List<LikeDto>> GetAllLikesByAccountOwnerId(int id)
    {
        List<Like> likes =  await _repository.GetAllLikesByAccountId(id);
        return likes.ParseToModels();
    }

    public async Task<bool> IsExist(Like model)
    {
        List<LikeDto> allLikes = await GetAllModels();
        LikeDto? foundLike = allLikes
            .Find(like => like.AccountSenderId == model.AccountSenderId
                          && like.PostReceiverId == model.PostReceiverId);

        return foundLike is not null;
    }
}