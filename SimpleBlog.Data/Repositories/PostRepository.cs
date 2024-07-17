using Microsoft.EntityFrameworkCore;
using SafeResult;
using SimpleBlog.Data.Context;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories.Abstract;

namespace SimpleBlog.Data.Repositories;

public class PostRepository : IRepository<Post>
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SafeResult<Post>>> GetAll()
    {
        return await _context.Posts.ToSafeResultListAsync();
    }

    public async Task<SafeResult<Post>> GetById(int id)
    {
        Post? post = await _context
            .Posts
            .FirstOrDefaultAsync(l => l.PostId == id);
        
        return post is null 
            ? SafeResult<Post>.GetErrorInstance("Not Found") 
            : SafeResult<Post>.GetResultInstance(post);
    }

    public async Task Add(Post entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Post entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Post entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }
}