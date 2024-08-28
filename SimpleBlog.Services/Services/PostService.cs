using SimpleBlog.Common.DTOs;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories;
using SimpleBlog.Data.Repositories.Abstract;
using SimpleBlog.Services.Extensions;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.Services.Services;

public class PostService : IPostService
{
    private IPostRepository _repository;

    public PostService(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PostDto>> GetAllModels()
    {
        List<Post> posts = await _repository.GetAll();
        return posts.ParseToModels();
    }

    public async Task<PostDto?> GetModelById(int id)
    {
        Post? post = await _repository.GetById(id);
        return post is not null
            ? post.ParseToDto()
            : throw new ArgumentException("This Post not exist");
    }

    public async Task<List<PostDto>> GetPostsByAccountId(int id)
    {
        List<Post> posts = await _repository.GetAllByAccountId(id);
        return posts is not null
            ? posts.ParseToModels()
            : throw new ArgumentException("This Post not exist");
    }

    public async Task<PostDto> AddModel(PostDto model)
    {
        Post post = model.ParseToEntity();
        await _repository.Add(post);
        return post.ParseToDto();
    }

    public async Task<PostDto?> UpdateModel(PostDto model)
    {
        Post? post = await _repository.GetById(model.PostId);

        if (post is null)
            throw new ArgumentException("This Post not found!");

        Post entity = model.ParseToEntity();
        await _repository.Update(entity);

        return entity.ParseToDto();
    }

    public async Task<string> DeleteModel(int id)
    {
        Post? post = await _repository.GetById(id);

        if (post is null)
            throw new ArgumentException($"Post with id - {id} does not exist");
        
        await _repository.Delete(post);
        
        return "Deleted successfully";
    }
}