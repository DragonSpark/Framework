using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	public sealed class AuthenticationState<T> : AuthenticationState where T : class
	{
		public static AuthenticationState<T> Default { get; } = new AuthenticationState<T>();

		AuthenticationState() : this(new ClaimsPrincipal(new ClaimsIdentity()), default) {}

		public AuthenticationState(ClaimsPrincipal user, T? profile) : base(user) => Profile = profile;

		public T? Profile { get; }

		public void Deconstruct(out T? profile, out ClaimsPrincipal principal)
		{
			profile   = Profile;
			principal = User;
		}
	}
}