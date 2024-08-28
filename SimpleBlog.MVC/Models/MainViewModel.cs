using FluentValidation.Results;

namespace SimpleBlog.MVC.Models;

public class MainViewModel<T>
{
    public ValidationResult Result { get; set; }
    public T Model { get; set; }

    
    
    public MainViewModel(T model, ValidationResult? result = null)
    {
        Model = model;
        Result = result ?? new();
    }
}