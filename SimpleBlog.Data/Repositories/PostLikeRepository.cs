using Microsoft.EntityFrameworkCore;
using SimpleBlog.Data.Context;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories.Abstract;

namespace SimpleBlog.Data.Repositories;

public class PostLikeRepository : IPostLikeRepository
{
    private readonly AppDbContext _context;

    public PostLikeRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<PostLike>> GetAll()
    {
        return await _context.PostLikes.AsNoTracking().ToListAsync();
    }

    public async Task<PostLike?> GetById(int id)
    {
        PostLike? postLike = await _context
            .PostLikes
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.PostLikeId == id);
        
        return postLike;
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

    public async Task<List<PostLike>> GetAllPostLikeByPostId(int id)
    {
        return await _context.PostLikes
            .AsNoTracking()
            .Where(p => p.PostId == id).ToListAsync();
    }
}