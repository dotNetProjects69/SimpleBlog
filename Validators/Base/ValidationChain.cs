using SimpleBlog.Models;
using System.Runtime.CompilerServices;
using SimpleBlog.Models.Interfaces.AccountModelParts;

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

        public ErrorModel Validate(IAccountModelPart model)
        {
            if (_firstValidator != null)
            {
                return _firstValidator.Validate(model);
            }

            return new(); // Если цепочка пуста, возвращаем пустую модель ошибок
        }
    }
}
