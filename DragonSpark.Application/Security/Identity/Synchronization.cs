using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public readonly struct Synchronization<T> where T : IdentityUser
	{
		public Synchronization(ExternalLoginInfo login, AuthenticationState<T> profile, ClaimsPrincipal source)
		{
			Login   = login;
			Profile = profile;
			Source  = source;
		}

		public ExternalLoginInfo Login { get; }

		public AuthenticationState<T> Profile { get; }

		public ClaimsPrincipal Source { get; }

		public void Deconstruct(out ExternalLoginInfo login, out AuthenticationState<T> profile,
		                        out ClaimsPrincipal source)
		{
			login   = Login;
			profile = Profile;
			source  = Source;
		}
	}
}