using System.Runtime.CompilerServices;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Models.Interfaces;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Validators.Base
{
    public interface IValidator<T>
    {
        public IErrorModel Validate(IAccountModelPart model);
        public IValidator<T> SetNext(IValidator<T> nextValidator);
    }
}
