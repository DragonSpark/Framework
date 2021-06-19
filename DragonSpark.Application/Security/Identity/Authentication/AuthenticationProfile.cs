using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class AuthenticationProfile<T> : IAuthenticationProfile where T : class
	{
		readonly SignInManager<T> _authentication;

		public AuthenticationProfile(SignInManager<T> authentication) => _authentication = authentication;

		public ValueTask<ExternalLoginInfo?> Get() => _authentication.GetExternalLoginInfoAsync().ToOperation();
	}
}