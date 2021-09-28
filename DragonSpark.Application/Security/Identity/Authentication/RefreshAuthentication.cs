using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class RefreshAuthentication<T> : IRefreshAuthentication<T> where T : IdentityUser
	{
		readonly IAuthentications<T> _authentications;
		readonly Compositions        _compositions;

		public RefreshAuthentication(IAuthentications<T> authentications, Compositions compositions)
		{
			_authentications = authentications;
			_compositions    = compositions;
		}

		public async ValueTask Get(T parameter)
		{
			var authentication = await _compositions.Await();
			if (authentication != null)
			{
				var (properties, claims) = authentication.Value;
				using var authentications = _authentications.Get();
				await authentications.Subject.SignInWithClaimsAsync(parameter, properties, claims.Open())
				                     .ConfigureAwait(false);
			}
		}
	}
}