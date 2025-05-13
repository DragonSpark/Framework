using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

public sealed class ApplicationUserAwareBearer : Validated<ClaimsPrincipal, string?>
{
	public ApplicationUserAwareBearer(IBearer bearer)
		: base(IsApplicationPrincipal.Default.Get, PrincipalIdentity.Default.Then().Select(bearer)) {}
}