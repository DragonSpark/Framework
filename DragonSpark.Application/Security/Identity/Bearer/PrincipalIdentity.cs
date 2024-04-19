using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Execution;
using System.Linq;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

public sealed class PrincipalIdentity : Select<ClaimsPrincipal, ClaimsIdentity>
{
	public static PrincipalIdentity Default { get; } = new();

	PrincipalIdentity() : base(x => x.Identity as ClaimsIdentity ?? x.Identities.First()) {}
}

// TODO

public sealed class ApplicationUserAwareBearer : Validated<ClaimsPrincipal, string?>
{
	public ApplicationUserAwareBearer(IBearer bearer)
		: base(IsApplicationPrincipal.Default.Get, PrincipalIdentity.Default.Then().Select(bearer)) {}
}

public sealed class AmbientBearer : Logical<string>
{
	public static AmbientBearer Default { get; } = new();

	AmbientBearer() {}
}