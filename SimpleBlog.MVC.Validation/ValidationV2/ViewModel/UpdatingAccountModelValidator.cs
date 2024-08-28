using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using SimpleBlog.Common.DTOs;
using SimpleBlog.MVC.Validation.ViewModels.Account;
using SimpleBlog.Services.Encryption;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.MVC.Validation.ValidationV2.ViewModel;

public class UpdatingAccountModelValidator : AbstractValidator<UpdatingAccountModel>
{
    public UpdatingAccountModelValidator(IAccountService accountService)
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not a valid email address.")
            .Must((model, _) => IsEmailUnique(model, accountService).Result).WithMessage("Email already used.");

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Name is null!")
            .NotEmpty().WithMessage("Name required")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Name can only contain letters and numbers.");
        
        RuleFor(x => x.Surname)
            .Cascade(CascadeMode.Stop)
            .Matches("[^a-zA-z0-9]*$").WithMessage("Name can only contain letters and numbers.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Password is null")
            .Must(password => !password.Contains(' ')).WithMessage("Password cannot contain spaces.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.Nickname)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Nickname is null")
            .NotEmpty().WithMessage("Nickname is empty")
            .Matches("^[a-z0-9_]+$").WithMessage("Nickname must contains only numbers, small letters and _")
            .Must((model, _) => IsNicknameUnique(model, accountService).Result).WithMessage("Nickname already used");
    }

    private async Task<bool> IsEmailUnique(UpdatingAccountModel model, IAccountService accountService)
    {
        List<AccountDto> accountDtos = await accountService.GetAllModelsByEmail(model.Email);
        return accountDtos.Any(accountDto => accountDto.AccountId != model.Id);
    }

    private async Task<bool> IsNicknameUnique(UpdatingAccountModel model, IAccountService accountService)
    {
        List<AccountDto> accountDtos = await accountService.GetAllModelsByNickname(model.Nickname);
        return accountDtos.Any(accountDto => accountDto.Nickname != model.Nickname);
    }
}