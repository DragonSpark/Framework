using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

sealed class CurrentPrincipal : Instance<ClaimsPrincipal>, ICurrentPrincipal
{
	public static CurrentPrincipal Default { get; } = new();

	CurrentPrincipal() : base(AnonymousPrincipal.Default) {}
}