using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public class Issue<T> : Resulting<string> where T : Nonce
{
    protected Issue(ICurrentContext context, CreateNonce<T> create) : base(context.Then().Select(create)) {}
}