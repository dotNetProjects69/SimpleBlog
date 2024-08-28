using SimpleBlog.Common.DTOs;

namespace SimpleBlog.Services.Services.Abstract;

public interface IAccountService : IService<AccountDto>
{
    public Task<List<AccountDto>> GetAllModelsByNickname(string nickname);
    public Task<List<AccountDto>> GetAllModelsByEmail(string email);
}