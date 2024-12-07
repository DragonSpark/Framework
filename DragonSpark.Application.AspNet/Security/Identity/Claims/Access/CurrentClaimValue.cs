using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Security.Identity.Claims.Access;

public class CurrentClaimValue : Result<string>
{
	protected CurrentClaimValue(ICurrentPrincipal source, IReadClaim read) : this(source, read, x => x.Value()) {}

	protected CurrentClaimValue(ICurrentPrincipal source, IReadClaim read, Func<Accessed, string> select)
		: base(source.Then().Select(read).Select(select)) {}
}