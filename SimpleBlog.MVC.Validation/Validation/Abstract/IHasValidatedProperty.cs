using SimpleBlog.MVC.Validation.Validation.Enums;

namespace SimpleBlog.MVC.Validation.Validation.Abstract;

public interface IHasValidatedProperty
{
    public ValidatedProperty ValidatedProperty { get; }
}