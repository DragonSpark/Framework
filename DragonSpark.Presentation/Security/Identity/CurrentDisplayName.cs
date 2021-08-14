using DragonSpark.Application;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Claims.Access;
using DisplayName = DragonSpark.Application.Security.Identity.Claims.Access.DisplayName;

namespace DragonSpark.Presentation.Security.Identity
{
	public sealed class CurrentDisplayName : CurrentClaimValue
	{
		public CurrentDisplayName(ICurrentPrincipal source)
			: base(source, DisplayName.Default, x => x.ValueOrDefault(Anonymous.Default)) {}
	}
}