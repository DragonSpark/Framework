using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.Mobile.Security.Identity;

sealed class AnonymousPrincipal : Instance<ClaimsPrincipal>
{
	public static AnonymousPrincipal Default { get; } = new();

	AnonymousPrincipal() : base(new(new ClaimsIdentity())) {}
}