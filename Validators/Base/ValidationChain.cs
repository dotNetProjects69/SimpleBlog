using SimpleBlog.Models;
using System.Runtime.CompilerServices;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Models.Interfaces;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Validators.Base
{
    public class ValidationChain<T>
    {
        private IValidator<T>? _firstValidator;

        public IValidator<T> SetNext(IValidator<T> validator)
        {
            if (_firstValidator == null)
                _firstValidator = validator;
            else
                _firstValidator?.SetNext(validator);

            return _firstValidator;
        }

        public IErrorModel Validate(IAccountModelPart model)
        {
            return _firstValidator != null
                ? _firstValidator.Validate(model)
                : new ErrorModel(); // Если цепочка пуста, возвращаем пустую модель ошибок
        }
    }
}
