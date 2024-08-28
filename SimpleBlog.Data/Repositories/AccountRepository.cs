using Microsoft.EntityFrameworkCore;
using SimpleBlog.Data.Context;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories.Abstract;

namespace SimpleBlog.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Account>> GetAll()
    {
        return await _context.Accounts.AsNoTracking().ToListAsync();
    }

    public async Task<Account?> GetById(int id)
    {
        Account? account = await _context
            .Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AccountId == id);
        
        return account;
    }

    public async Task<List<Account>> GetByNickname(string nickname)
    {
        List<Account> account = await _context
            .Accounts
            .Where(a => a.Nickname == nickname)
            .ToListAsync();

        return account;
    }

    public async Task<List<Account>> GetByEmail(string email)
    {
        List<Account> accounts = await _context
            .Accounts
            .Where(a => a.Email == email)
            .ToListAsync();

        return accounts;
    }

    public async Task Add(Account entity)
    {
        _context.Accounts.Add(entity);
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