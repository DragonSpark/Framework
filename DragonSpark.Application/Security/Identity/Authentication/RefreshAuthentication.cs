using DragonSpark.Application.Security.Identity.Authentication.Persist;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class RefreshAuthentication<T> : IRefreshAuthentication<T> where T : IdentityUser
{
	readonly Compositions        _compositions;
	readonly IPersistRefresh<T>  _persist;

	public RefreshAuthentication(Compositions compositions, IPersistRefresh<T> persist)
	{
		_compositions    = compositions;
		_persist         = persist;
	}

	public async ValueTask Get(T parameter)
	{
		var composition = await _compositions.Await();
		if (composition != null)
		{
			var (properties, claims) = composition.Value;
			await _persist.Await(new PersistMetadataInput<T>(parameter, properties, claims));
		}
	}
}