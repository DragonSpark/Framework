using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class RefreshAuthentication<T> : IRefreshAuthentication<T> where T : IdentityUser
	{
		readonly SignInManager<T> _authentication;
		readonly Compositions          _compositions;

		public RefreshAuthentication(SignInManager<T> authentication, Compositions compositions)
		{
			_authentication = authentication;
			_compositions        = compositions;
		}

		public async ValueTask Get(T parameter)
		{
			var authentication = await _compositions.Await();
			if (authentication != null)
			{
				var (properties, claims) = authentication.Value;
				await _authentication.SignInWithClaimsAsync(parameter, properties, claims.Open()).ConfigureAwait(false);
			}
		}
	}
}