using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Runtime.CompilerServices;
using static SimpleBlog.Validators.Base.TypeTransformer;

namespace SimpleBlog.Validators.ValidatorType
{
    public class ModelHasNoBlankFields : Validator<IAccountModelPart>
    {
        private protected override IErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            ValidationChain<IAccountModelPart> chain = new();
            chain
                .SetNext(new NameMustNotEmpty())
                .SetNext(new EmailMustNotEmpty())
                .SetNext(new NicknameMustNotEmpty())
                .SetNext(new PasswordMustNotEmpty());

            return chain.Validate(baseModel);
        }
    }
}
