using System.Security.Claims;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class BearerIdentity : IAlteration<ClaimsIdentity>
{
	readonly BearerClaims _claims;

	public BearerIdentity(BearerClaims claims) => _claims = claims;

	public ClaimsIdentity Get(ClaimsIdentity parameter) => new(_claims.Get(parameter), parameter.AuthenticationType);
}