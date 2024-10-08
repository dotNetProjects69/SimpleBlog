﻿using SimpleBlog.Models;
using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.AccountModelParts;
using SimpleBlog.Validators.Base;
using System.Net;
using System.Runtime.CompilerServices;
using static SimpleBlog.Validators.Base.TypeTransformer;

[assembly: InternalsVisibleTo("SimpleBlogTests")]
namespace SimpleBlog.Validators.ValidatorType
{
    public class PasswordLengthEnough : Validator<IAccountModelPart>
    {
        private readonly int _length;

        public PasswordLengthEnough()
        {
            _length = 7;
        }

        public PasswordLengthEnough(int length)
        {
            _length = length;
        }

        private protected override IErrorModel ValidateLogic(IAccountModelPart baseModel)
        {
            IPassword? model = TryTransformTo<IPassword>(baseModel);
            return model.Password.Length >= _length
                ? ErrorModel.Success
                : new(HttpStatusCode.BadRequest, 
                      $"The password must be at least {_length} characters long");
        }
    }
}
