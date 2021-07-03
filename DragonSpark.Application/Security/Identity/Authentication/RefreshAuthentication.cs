using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class RefreshAuthentication<T> : IRefreshAuthentication<T> where T : IdentityUser
	{
		readonly SignInManager<T> _authentication;
		readonly Compose          _compose;

		public RefreshAuthentication(SignInManager<T> authentication, Compose compose)
		{
			_authentication = authentication;
			_compose        = compose;
		}

		public async ValueTask Get(T parameter)
		{
			var authentication = await _compose.Await();
			if (authentication != null)
			{
				var (properties, claims) = authentication.Value;
				await _authentication.SignInWithClaimsAsync(parameter, properties, claims.Open()).ConfigureAwait(false);
			}
		}
	}
}