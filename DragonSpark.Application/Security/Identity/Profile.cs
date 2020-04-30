using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	// TODO: AuthenticationState?
	public readonly struct Profile<T> where T : class
	{
		public static Profile<T> Default { get; } = new Profile<T>(new ClaimsPrincipal(new ClaimsIdentity()), null);

		public Profile(ClaimsPrincipal principal, T user)
		{
			User      = user;
			Principal = principal;
		}

		public ClaimsPrincipal Principal { get; }

		public T User { get; }

		public void Deconstruct(out T user, out ClaimsPrincipal principal)
		{
			user      = User;
			principal = Principal;
		}
	}
}