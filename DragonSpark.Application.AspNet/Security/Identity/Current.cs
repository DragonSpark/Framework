using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class Current<T> : Resulting<T>, ICurrent<T> where T : class
{
    public Current(CurrentContextUser<T> current, ICurrentContext context)
        : base(current.Then().Bind(context)) {}
}