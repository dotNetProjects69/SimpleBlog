using Microsoft.EntityFrameworkCore;
using SimpleBlog.Data.Context;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories.Abstract;

namespace SimpleBlog.Data.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly AppDbContext _context;

    public LikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Like>> GetAll()
    {
        return await _context.Likes.AsNoTracking().ToListAsync();
    }

    public async Task<Like?> GetById(int id)
    {
        Like? like = await _context
            .Likes
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.LikeId == id);

        return like;
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

    public async Task<List<Like>> GetAllLikesByAccountId(int id)
    {
        return await _context.Likes
            .AsNoTracking()
            .Where(l => l.AccountSenderId == id).ToListAsync();
    }
}