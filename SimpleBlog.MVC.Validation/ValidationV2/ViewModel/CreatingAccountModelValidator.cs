using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SimpleBlog.Common.DTOs;
using SimpleBlog.MVC.Validation.ViewModels.Authentication;
using SimpleBlog.Services.Encryption;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.MVC.Validation.ValidationV2.ViewModel;

public class CreatingAccountModelValidator : AbstractValidator<SignInModel>
{
    public CreatingAccountModelValidator(IAccountService accountService)
    {
        RuleFor(m => m.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Email is null")
            .NotEmpty().WithMessage("Email is empty")
            .EmailAddress().WithMessage("Incorrect email address")
            .Must((email) => EmailIsUsed(email, accountService).Result).WithMessage("Email already used");

        RuleFor(m => m.Password)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Password prop is null")
            .NotEmpty().WithMessage("Please enter password")
            .Must((model, _) => PasswordIsCorrect(model, accountService).Result)
            .WithMessage("Entered password is incorrect");
    }

    private async Task<bool> PasswordIsCorrect(SignInModel model, IAccountService accountService)
    {
        AccountDto? accountDto = (await accountService.GetAllModelsByEmail(model.Email))
            .FirstOrDefault();

        if (accountDto is null)
            return false;

        string hashedPassword = accountDto.Password;

        return Password.VerifyHashedPassword(hashedPassword, model.Password);
    }

    private async Task<bool> EmailIsUsed(string email, IAccountService accountService)
    {
        List<AccountDto> accountDtos = await accountService.GetAllModelsByEmail(email);

        return accountDtos.Count > 0;
    }
}