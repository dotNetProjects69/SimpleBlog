using Microsoft.EntityFrameworkCore;
using SimpleBlog.Data.Context;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories.Abstract;

namespace SimpleBlog.Data.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Post>> GetAll()
    {
        return _context.Posts.AsNoTracking().ToListAsync();
    }

    public async Task<List<Post>> GetAllByAccountId(int id)
    {
        return await _context.Posts.Where(p => p.Owner != null && p.Owner.AccountId == id).ToListAsync();
    }

    public async Task<Post?> GetById(int id)
    {
        Post? post = await _context
            .Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.PostId == id);

        return post;
    }

    public async Task Add(Post entity)
    {
        if (_context.Entry(entity.Owner).State == EntityState.Detached)
        {
            _context.Attach(entity.Owner);
        }
        
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