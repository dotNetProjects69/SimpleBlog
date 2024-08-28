using System.ComponentModel.DataAnnotations;
using SimpleBlog.MVC.Validation.Validation.Abstract;
using SimpleBlog.MVC.Validation.Validation.Enums;

namespace SimpleBlog.MVC.Validation.Validation.ValidationAttributes;

public class PasswordIsCorrect : ValidationAttribute, IHasValidatedProperty
{
    private readonly int _passwordLength;

    public ValidatedProperty ValidatedProperty { get; } = ValidatedProperty.Password;

    public PasswordIsCorrect(int length = 7)
    {
        _passwordLength = length;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return new($"{ValidatedProperty.ToString()} is null");

        if (value is not string s)
            return new($"{ValidatedProperty.ToString()} must be string");

        if (string.IsNullOrEmpty(s))
            return new($"{ValidatedProperty.ToString()} property is empty");

        if (s.Contains(' '))
            return new("Password must not contain gaps");

        return s.Length < _passwordLength
            ? new($"The password must be at least {_passwordLength} characters long")
            : ValidationResult.Success;
    }
}