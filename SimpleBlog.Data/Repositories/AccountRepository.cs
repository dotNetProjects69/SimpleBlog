using Microsoft.EntityFrameworkCore;
using SimpleBlog.Data.Context;
using SimpleBlog.Data.Entities;
using SafeResult;
using SimpleBlog.Data.Repositories.Abstract;

namespace SimpleBlog.Data.Repositories;

public class AccountRepository : IRepository<Account> 
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SafeResult<Account>>> GetAll()
    {
        return await _context.Accounts.ToSafeResultListAsync();
    }

    public async Task<SafeResult<Account>> GetById(int id)
    {
        Account? account = await _context
            .Accounts
            .FirstOrDefaultAsync(a => a.AccountId == id);
        return account is null 
            ? SafeResult<Account>.GetErrorInstance("Not Found") 
            : SafeResult<Account>.GetResultInstance(account);
    }

    public async Task Add(Account entity)
    {
        await _context.Accounts.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Account entity)
    {
        _context.Accounts.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Account entity)
    {
        _context.Accounts.Remove(entity);
        await _context.SaveChangesAsync();
    }
}