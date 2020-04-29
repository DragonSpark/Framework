using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public readonly struct Profile<T> where T : IdentityUser
	{
		public Profile(ClaimsPrincipal principal, T user)
		{
			User      = user;
			Principal = principal;
		}

		public T User { get; }

		public ClaimsPrincipal Principal { get; }

		public void Deconstruct(out T user, out ClaimsPrincipal principal)
		{
			user      = User;
			principal = Principal;
		}
	}
}