using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class CurrentContextSession<T> : Result<SignInManager<T>> where T : class
{
	public CurrentContextSession(IContextSession<T> session, ICurrentContext context)
		: base(session.Then().Bind(context)) {}
}