using Microsoft.EntityFrameworkCore;
using SafeResult;
using SimpleBlog.Data.Context;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories.Abstract;

namespace SimpleBlog.Data.Repositories;

public class PostLikeRepository : IRepository<PostLike>
{
    private readonly AppDbContext _context;

    public PostLikeRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<SafeResult<PostLike>>> GetAll()
    {
        return await _context.PostLikes.ToSafeResultListAsync();
    }

    public async Task<SafeResult<PostLike>> GetById(int id)
    {
        PostLike? postLike = await _context
            .PostLikes
            .FirstOrDefaultAsync(l => l.PostLikeId == id);
        
        return postLike is null 
            ? SafeResult<PostLike>.GetErrorInstance("Not Found") 
            : SafeResult<PostLike>.GetResultInstance(postLike);
    }

    public async Task Add(PostLike entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(PostLike entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(PostLike entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }
}