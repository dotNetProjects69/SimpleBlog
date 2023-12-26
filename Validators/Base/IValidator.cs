using SimpleBlog.Models;
using System.Runtime.CompilerServices;
using SimpleBlog.Models.Interfaces.AccountModelParts;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Validators.Base
{
    public interface IValidator<T>
    {
        public ErrorModel Validate(IAccountModelPart model);
        public IValidator<T> SetNext(IValidator<T> nextValidator);
    }
}
