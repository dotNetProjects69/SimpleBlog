using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using SimpleBlog.Common.DTOs;
using SimpleBlog.MVC.Validation.ViewModels.Account;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.MVC.Validation.ValidationV2.ViewModel;

public class SignUpModelValidator : AbstractValidator<CreatingAccountModel>
{
    public SignUpModelValidator(IAccountService accountService)
    {
        RuleFor(m => m.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Name prop is null")
            .NotEmpty().WithMessage("Name can not be empty")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Name must contains only letters and numbers");

        RuleFor(m => m.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Email is null")
            .NotEmpty().WithMessage("Email can not be empty")
            .EmailAddress().WithMessage("Email address is incorrect")
            .Must(email => EmailIsUnique(email, accountService).Result).WithMessage("Email already used");

        RuleFor(m => m.Nickname)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Nickname prop is null")
            .NotEmpty().WithMessage("Nickname is empty")
            .Matches("^[a-z0-9]*$").WithMessage("Nickname should only include lowercase letters and numbers")
            .Must(nickname => NicknameIsUnique(nickname, accountService).Result).WithMessage("Nickname already used");

        RuleFor(m => m.Password)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Password is null")
            .Must(password => string.IsNullOrEmpty(password) || IsPasswordValid(password))
            .WithMessage("Password must be at least 8 characters long and cannot contain spaces");

        RuleFor(m => m.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Confirm password is null")
            .NotEmpty().WithMessage("Confirm password is empty")
            .Must((model, confirmPassword) => model.Password == confirmPassword)
            .WithMessage("Passwords don't match. Please make sure both fields are the same");
    }

    private bool IsPasswordValid(string password) => password.Length >= 8 && !password.Contains(' ');

    private async Task<bool> NicknameIsUnique(string nickname, IAccountService accountService)
    {
        List<AccountDto> accountDtos = await accountService.GetAllModelsByNickname(nickname);
        return accountDtos.Count == 0;
    }

    private async Task<bool> EmailIsUnique(string email, IAccountService accountService)
    {
        List<AccountDto> accountDtos = await accountService.GetAllModelsByEmail(email);
        return accountDtos.Count == 0;
    }
}