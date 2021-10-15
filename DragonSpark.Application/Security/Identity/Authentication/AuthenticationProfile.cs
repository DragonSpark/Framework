using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class AuthenticationProfile<T> : IAuthenticationProfile where T : class
{
	readonly IAuthentications<T> _authentications;

	public AuthenticationProfile(IAuthentications<T> authentications) => _authentications = authentications;

	public async ValueTask<ExternalLoginInfo?> Get()
	{
		using var authentication = _authentications.Get();
		var       result         = await authentication.Subject.GetExternalLoginInfoAsync().ConfigureAwait(false);
		return result;
	}
}