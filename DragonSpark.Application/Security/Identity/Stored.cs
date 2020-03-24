using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public readonly struct Stored<T> where T : IdentityUser
	{
		public Stored(T user, ClaimsPrincipal principal)
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