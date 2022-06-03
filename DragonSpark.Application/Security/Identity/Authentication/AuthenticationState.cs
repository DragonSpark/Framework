using DragonSpark.Compose;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class AuthenticationState<T> : AuthenticationState where T : IdentityUser
{
	readonly T? _profile;
	public static AuthenticationState<T> Default { get; } = new();

	AuthenticationState() : this(new ClaimsPrincipal(new ClaimsIdentity()), default) {}

	public AuthenticationState(ClaimsPrincipal user, T? profile) : base(user)
	{
		_profile = profile;
	}

	public T? Profile => _profile?.Reference().To<T>();

	public void Deconstruct(out ClaimsPrincipal principal, out T? profile)
	{
		principal = User;
		profile   = _profile;
	}
}