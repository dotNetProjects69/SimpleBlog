using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SimpleBlog.MVC.Validation.Validation.Abstract;
using SimpleBlog.MVC.Validation.Validation.Enums;
using SimpleBlog.MVC.Validation.ViewModels;

namespace SimpleBlog.MVC.Validation.Validation.Validator;

public class AccountValidator
{
    private readonly IServiceProvider _serviceProvider;

    public AccountValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public List<ValidationErrorModel> Validate(object obj)
    {
        List<ValidationErrorModel> results = [];
        ValidationContext context = new(obj, _serviceProvider, items: null);
        ValidatedProperty validatedProperty = ValidatedProperty.None;

        foreach (PropertyInfo property in obj
                     .GetType()
                     .GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            object? value = property.GetValue(obj);


            foreach (object attribute in property.GetCustomAttributes(true))
            {

                if (attribute is IHasValidatedProperty hasValidatedProperty) 
                    validatedProperty = hasValidatedProperty.ValidatedProperty;
                
                if (attribute is ValidationAttribute validationAttribute)
                {
                    ValidationResult? result = validationAttribute.GetValidationResult(value, context);

                    if (result is null)
                        continue;

                    if (result != ValidationResult.Success)
                        results.Add(new(validatedProperty, result));
                }
            }
        }

        return results;
    }
}