using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public readonly struct Destination<T> where T : IdentityUser
	{
		public Destination(UserManager<T> manager, T user, ClaimsPrincipal principal)
		{
			Manager   = manager;
			User      = user;
			Principal = principal;
		}

		public UserManager<T> Manager { get; }

		public T User { get; }

		public ClaimsPrincipal Principal { get; }

		public void Deconstruct(out UserManager<T> manager, out T user, out ClaimsPrincipal principal)
		{
			manager   = Manager;
			user      = User;
			principal = Principal;
		}
	}
}