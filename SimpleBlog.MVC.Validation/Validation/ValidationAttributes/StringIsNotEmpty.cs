using System;
using System.ComponentModel.DataAnnotations;
using SimpleBlog.MVC.Validation.Validation.Abstract;
using SimpleBlog.MVC.Validation.Validation.Enums;

namespace SimpleBlog.MVC.Validation.Validation.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class StringIsNotEmpty : ValidationAttribute, IHasValidatedProperty
{
    public ValidatedProperty ValidatedProperty { get; }

    public StringIsNotEmpty(ValidatedProperty validatedProperty)
    {
        ValidatedProperty = validatedProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return new($"{ValidatedProperty.ToString()} value is null");

        if (value is not string s)
            return new($"{ValidatedProperty.ToString()} must be string");

        return string.IsNullOrEmpty(s.Trim())
            ? new($"{ValidatedProperty.ToString()} is empty")
            : ValidationResult.Success;
    }
}