using Microsoft.EntityFrameworkCore;
using SafeResult;
using SimpleBlog.Data.Context;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories.Abstract;

namespace SimpleBlog.Data.Repositories;

public class LikeRepository : IRepository<Like>
{
    private readonly AppDbContext _context;

    public LikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SafeResult<Like>>> GetAll()
    {
        return await _context.Likes.ToSafeResultListAsync();
    }

    public async Task<SafeResult<Like>> GetById(int id)
    {
        Like? like = await _context
            .Likes
            .FirstOrDefaultAsync(l => l.LikeId == id);
        
        return like is null 
            ? SafeResult<Like>.GetErrorInstance("Not Found") 
            : SafeResult<Like>.GetResultInstance(like);
    }

    public async Task Add(Like entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Like entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Like entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }
}