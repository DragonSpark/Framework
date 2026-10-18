using DragonSpark.Compose;
using DragonSpark.Composition;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Entra;

using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

sealed class EasyAuthClaimsTransformation<T> : IClaimsTransformation where T : class
{
	readonly IUserClaimsPrincipalFactory<T> _claims;
	readonly LocateEasyAuthUser<T>          _locate;
	readonly Claim                          _marker;

	public EasyAuthClaimsTransformation(IUserClaimsPrincipalFactory<T> claims, LocateEasyAuthUser<T> locate)
		: this(claims, locate, MarkerClaim.Default) {}

	[Candidate(false)]
	public EasyAuthClaimsTransformation(IUserClaimsPrincipalFactory<T> claims, LocateEasyAuthUser<T> locate,
	                                    Claim marker)
	{
		_claims = claims;
		_locate = locate;
		_marker = marker;
	}

	public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		if (await _locate.Off(principal) is {} user
		    && await _claims.CreateAsync(user).Off() is { Identity: ClaimsIdentity identity })
		{
			identity.AddClaim(_marker);

			return new(new[] { identity }.Concat(principal.Identities));
		}

		return principal;
	}
}