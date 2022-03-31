using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

sealed class CurrentUserIdentity : ICurrentUserIdentity
{
	readonly ICurrentPrincipal _principal;

	public CurrentUserIdentity(ICurrentPrincipal principal) => _principal = principal;

	public string Get() => _principal.Get().FindFirstValue(ClaimTypes.NameIdentifier);
}