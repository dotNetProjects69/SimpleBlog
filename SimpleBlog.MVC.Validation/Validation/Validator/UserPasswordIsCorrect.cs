using System.Threading.Tasks;
using SimpleBlog.Common.DTOs;
using SimpleBlog.Services.Encryption;
using SimpleBlog.Services.Services;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.MVC.Validation.Validation.Validator;

public class UserPasswordIsCorrect
{
    private IAccountService _service;

    public UserPasswordIsCorrect(IAccountService service)
    {
        _service = service;
    }

    public async Task<bool> ValidateByNickname(string nickname, string password)
    {
        AccountDto? accountDto = await _service.GetAllModelsByNickname(nickname);

        if (accountDto is null)
            return false;

        string hashedPassword = accountDto.Password;

        return Password.VerifyHashedPassword(hashedPassword, password);
    }

    public async Task<bool> ValidateByEmail(string email, string password)
    {
        AccountDto? accountDto = await _service.FindAllModelsByEmail(email);

        if (accountDto is null)
            return false;

        string hashedPassword = accountDto.Password;

        return Password.VerifyHashedPassword(hashedPassword, password);
    }
}