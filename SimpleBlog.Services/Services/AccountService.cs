using SimpleBlog.Common.DTOs;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories.Abstract;
using SimpleBlog.Services.Extensions;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.Services.Services;

public class AccountService : IAccountService
{
    private IAccountRepository _repository;

    public AccountService(IAccountRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<List<AccountDto>> GetAllModels()
    {
        List<Account> accounts = await _repository.GetAll();
        return accounts.ParseToDto();
    }

    public async Task<AccountDto?> GetModelById(int id)
    {
        Account? account = await _repository.GetById(id);

        return account is not null
            ? account.ParseToDto()
            : throw new ArgumentException("This account not found!");
    }

    public async Task<List<AccountDto>> GetAllModelsByNickname(string nickname)
    {
        List<Account> account = await _repository.GetByNickname(nickname);

        return account.ParseToDto();
    }

    public async Task<List<AccountDto>> GetAllModelsByEmail(string email)
    {
        List<Account> account = await _repository.GetByEmail(email);

        return account.ParseToDto();
    }

    public async Task<AccountDto> AddModel(AccountDto model)
    {
        List<AccountDto> allAccounts = await GetAllModels();
        AccountDto? foundAccount = allAccounts.Find(account => account.Nickname == model.Nickname);
        
        if (foundAccount is not null)
            throw new ArgumentException("Account with this Nickname is exist!");

        Account entity = model.ParseToEntity();
        
        await _repository.Add(entity);

        return entity.ParseToDto();
    }

    public async Task<AccountDto?> UpdateModel(AccountDto model)
    {
        Account? account = await _repository.GetById(model.AccountId);

        if (account is null)
            throw new ArgumentException("This account not found!");

        Account entity = model.ParseToEntity();
        await _repository.Update(entity);

        return entity.ParseToDto();
    }

    public async Task<string> DeleteModel(int id)
    {
        Account? account = await _repository.GetById(id);

        if (account is null)
            throw new ArgumentException("This account not found!");
        
        await _repository.Delete(account);
        
        return "Deleted successfully";
    }
}