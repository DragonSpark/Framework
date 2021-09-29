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
			var composition = await _compositions.Await();
			if (composition != null)
			{
				var (properties, claims) = composition.Value;
				using var authentication = _authentications.Get();
				await authentication.Subject.SignInWithClaimsAsync(parameter, properties, claims.Open())
				                    .ConfigureAwait(false);
			}
		}
	}
}