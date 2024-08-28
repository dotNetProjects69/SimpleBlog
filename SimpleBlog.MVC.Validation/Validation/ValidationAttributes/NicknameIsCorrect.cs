using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using SimpleBlog.MVC.Validation.Validation.Abstract;
using SimpleBlog.MVC.Validation.Validation.Enums;

namespace SimpleBlog.MVC.Validation.Validation.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class NicknameIsCorrect : ValidationAttribute, IHasValidatedProperty
{
    public ValidatedProperty ValidatedProperty { get; } = ValidatedProperty.Nickname;
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return new($"{ValidatedProperty.ToString()} is null");

        if (value is not string s)
            return new($"{ValidatedProperty.ToString()} must be a string");
        
        
        return IsValidNickname(s)
            ? ValidationResult.Success 
            : new("Nickname must contains only a-z, 0-9 and _");
    }

    private bool IsValidNickname(string password)
    {
        string pattern = "^[a-z0-9_]+$";
        bool isValid = Regex.IsMatch(password, pattern);
        return isValid;
    }
}