using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity.Claims.Access
{
	public class CurrentClaimValue : Result<string>
	{
		protected CurrentClaimValue(ICurrentPrincipal source, IReadClaim read)
			: base(source.Then().Select(read).Select(x => x.Value())) {}
	}
}