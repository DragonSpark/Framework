using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

sealed class CurrentUserIdentity : ICurrentUserIdentity
{
	readonly ICurrentPrincipal _principal;

	public CurrentUserIdentity(ICurrentPrincipal principal) => _principal = principal;

	public string Get() => _principal.Get().FindFirstValue(ClaimTypes.NameIdentifier).Verify();
}

// TODO
public interface ICurrentUserNumber : IResult<uint> {}

sealed class CurrentUserNumber : ICurrentUserNumber
{
	readonly ICurrentUserIdentity _identity;

	public CurrentUserNumber(ICurrentUserIdentity identity) => _identity = identity;

	public uint Get() => uint.Parse(_identity.Get());
}