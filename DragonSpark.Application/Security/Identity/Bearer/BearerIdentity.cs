using DragonSpark.Model.Selection.Alterations;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class BearerIdentity : IAlteration<ClaimsIdentity>
{
	readonly BearerClaims _claims;

	public BearerIdentity(BearerClaims claims) => _claims = claims;

	public ClaimsIdentity Get(ClaimsIdentity parameter) => new(_claims.Get(parameter));
}