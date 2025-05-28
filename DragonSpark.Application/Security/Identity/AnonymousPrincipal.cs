using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

public sealed class AnonymousPrincipal : Instance<ClaimsPrincipal>
{
	public static AnonymousPrincipal Default { get; } = new();

	AnonymousPrincipal() : base(new(new ClaimsIdentity())) {}
}