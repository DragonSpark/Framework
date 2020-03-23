using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public readonly struct Synchronization<T> where T : IdentityUser
	{
		public Synchronization(ExternalLoginInfo login, Stored<T> stored, ClaimsPrincipal source)
		{
			Login  = login;
			Stored = stored;
			Source = source;
		}

		public ExternalLoginInfo Login { get; }

		public Stored<T> Stored { get; }

		public ClaimsPrincipal Source { get; }

		public void Deconstruct(out ExternalLoginInfo login, out Stored<T> stored, out ClaimsPrincipal source)
		{
			login  = Login;
			stored = Stored;
			source = Source;
		}
	}
}