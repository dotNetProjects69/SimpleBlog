using System;
using System.ComponentModel.DataAnnotations;
using SimpleBlog.Common.DTOs;
using SimpleBlog.MVC.Validation.Validation.Enums;
using SimpleBlog.Services.Services;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.MVC.Validation.Validation.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class NicknameInUse : ValidationAttribute
{
    public ValidatedProperty ValidatedProperty { get; } = ValidatedProperty.Nickname;
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return new($"{ValidatedProperty.ToString()} is null");

        if (value is not string s)
            return new($"{ValidatedProperty.ToString()} must be string");
        
        if (string.IsNullOrEmpty(s.Trim()))
            return new($"{ValidatedProperty.ToString()} is empty");

        if (validationContext.GetService(typeof(IAccountService)) is not AccountService service)
            return new("Account Service not found.");
        
        AccountDto? accountDto =  service.GetAllmodelsByNickname(s).Result;

        return accountDto is not null
            ? ValidationResult.Success 
            : new($"Account with this nickname: {s} does not exist.");
    }
}