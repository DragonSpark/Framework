using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class RefreshAuthentication<T> : IRefreshAuthentication<T> where T : IdentityUser
	{
		readonly SignInManager<T> _authentication;
		readonly Authenticate     _authenticate;

		public RefreshAuthentication(SignInManager<T> authentication, Authenticate authenticate)
		{
			_authentication = authentication;
			_authenticate   = authenticate;
		}

		public async ValueTask Get(T parameter)
		{
			var authentication = await _authenticate.Await();
			if (authentication != null)
			{
				var (properties, claims) = authentication.Value;
				await _authentication.SignInWithClaimsAsync(parameter, properties, claims.Open());
			}
		}
	}
}