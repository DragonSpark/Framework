using System.Security.Claims;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity;

sealed class AnonymousPrincipal : Instance<ClaimsPrincipal>
{
	public static AnonymousPrincipal Default { get; } = new();

	AnonymousPrincipal() : base(new(new ClaimsIdentity())) {}
}