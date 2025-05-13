using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class CurrentBearer : Result<string>, ICurrentBearer
{
	public CurrentBearer(ICurrentPrincipal principal, IBearer bearer) : base(principal.Then().Select(bearer.Get)) {}
}