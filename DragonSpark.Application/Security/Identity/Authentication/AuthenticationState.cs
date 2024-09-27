using DragonSpark.Compose;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class AuthenticationState<T> : AuthenticationState where T : IdentityUser
{
	public static implicit operator int(AuthenticationState<T> instance) => instance.Profile.Verify().Id;

	public static implicit operator uint(AuthenticationState<T> instance) => instance.Profile.Verify().Id.Grade();

	public static implicit operator T(AuthenticationState<T> instance) => instance.Profile.Verify();

	public static implicit operator ClaimsPrincipal(AuthenticationState<T> instance) => instance.User;

	public static AuthenticationState<T> Default { get; } = new();

	AuthenticationState() : this(new(new ClaimsIdentity()), default) {}

	public AuthenticationState(ClaimsPrincipal user, T? profile) : base(user) => Profile = profile;

	public T? Profile { get; }

	public void Deconstruct(out ClaimsPrincipal principal, out T? profile)
	{
		principal = User;
		profile   = Profile;
	}
}