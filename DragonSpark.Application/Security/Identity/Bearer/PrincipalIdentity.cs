using DragonSpark.Model.Selection;
using System.Linq;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

public sealed class PrincipalIdentity : Select<ClaimsPrincipal, ClaimsIdentity>
{
	public static PrincipalIdentity Default { get; } = new();

	PrincipalIdentity() : base(x => x.Identity as ClaimsIdentity ?? x.Identities.First()) {}
}