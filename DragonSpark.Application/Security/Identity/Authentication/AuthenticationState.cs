using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class AuthenticationState<T> : AuthenticationState where T : class
{
	public static AuthenticationState<T> Default { get; } = new();

	AuthenticationState() : this(new ClaimsPrincipal(new ClaimsIdentity()), default) {}

	public AuthenticationState(ClaimsPrincipal user, T? profile) : base(user) => Profile = profile;

	public T? Profile { get; }

	public void Deconstruct(out ClaimsPrincipal principal, out T? profile)
	{
		profile   = Profile;
		principal = User;
	}
}