using System.ComponentModel.DataAnnotations;
using SimpleBlog.MVC.Validation.Validation.Enums;

namespace SimpleBlog.MVC.Validation.ViewModels;

public class ValidationErrorModel
{
    public ValidatedProperty ValidatedProperty { get; }
    
    public ValidationResult? ValidationResult { get; }

    public bool IsSuccess => ValidationResult == ValidationResult.Success;

    public ValidationErrorModel(ValidatedProperty validatedProperty, ValidationResult validationResult)
    {
        ValidationResult = validationResult;
        ValidatedProperty = validatedProperty;
    }

    public ValidationErrorModel()
    {
        ValidatedProperty = ValidatedProperty.None;
        ValidationResult = ValidationResult.Success;
    }
}