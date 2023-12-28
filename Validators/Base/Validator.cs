using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Validators.Base
{
    public abstract class Validator<T>: IValidator<T>
    {
        private IValidator<T>? _nextValidator;

        public IValidator<T> SetNext(IValidator<T> nextValidator)
        {
            _nextValidator = nextValidator;
            return nextValidator;
        }

        public ErrorModel Validate(IAccountModelPart baseModel)
        {
            ErrorModel result = ValidateLogic(baseModel);

            if (result.StatusCodeIsNotOk())
                return result;

            return _nextValidator is not null
                ? _nextValidator.Validate(baseModel)
                : new(); // Возвращаем пустую модель ошибок в конце цепочки
        }

        private protected abstract ErrorModel ValidateLogic(IAccountModelPart model);
    }
}
