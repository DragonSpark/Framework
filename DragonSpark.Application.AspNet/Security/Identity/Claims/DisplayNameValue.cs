using DragonSpark.Compose;
using DragonSpark.Text;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims;

sealed class DisplayNameValue : IFormatter<(ClaimsPrincipal Principal, string Provider)>
{
	readonly IDisplayNameClaim _claim;

	public DisplayNameValue(IDisplayNameClaim claim) => _claim = claim;

	public string Get((ClaimsPrincipal Principal, string Provider) parameter)
	{
		var (principal, provider) = parameter;

		var claim = _claim.Get(provider);
		var result = principal.HasClaim(claim)
			             ? principal.FindFirstValue(claim).NullIfEmpty() ?? principal.UserName()
			             : principal.UserName();
		return result;
	}
}