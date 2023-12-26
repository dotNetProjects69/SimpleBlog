using SimpleBlog.Models.Interfaces.AccountModelParts;

namespace SimpleBlog.Validators.Base
{
    public static class TypeTransformer
    {
        internal static T TryTransformTo<T>(IAccountModelPart baseModel)
        {
            return TryTransformT1ToT2<IAccountModelPart, T>(baseModel);
        }

        private static T2 TryTransformT1ToT2<T1, T2>(T1 baseModel)
        {
            if (baseModel is not T2 model)
                throw new ArgumentException($"Base model is not type of {typeof(T2)}");
            return model;
        }
    }
}
