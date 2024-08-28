using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using SimpleBlog.MVC.Validation.Validation.Abstract;
using SimpleBlog.MVC.Validation.Validation.Enums;

namespace SimpleBlog.MVC.Validation.Validation.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class EmailIsCorrect : ValidationAttribute, IHasValidatedProperty
{
    public ValidatedProperty ValidatedProperty { get; } = ValidatedProperty.Email;
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return new($"{ValidatedProperty.ToString()} is null");

        if (value is not string s)
            return new($"{ValidatedProperty.ToString()} must be a string");
        
        return IsValidEmail(s) 
            ? ValidationResult.Success 
            : new("Email is not valid");
    }
    
    private bool IsValidEmail(string email)
    {
        string trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith('.'))
            return false; // suggested by @TK-421

        try
        {
            MailAddress emailAddress = new (email);
            return emailAddress.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
}