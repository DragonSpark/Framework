using DragonSpark.Application.Security.Identity.Authentication;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity;

sealed class HasValidState<T> : IHasValidState<T> where T : IdentityUser
{
	readonly IAuthentications<T> _authentications;

	public HasValidState(IAuthentications<T> authentications) => _authentications = authentications;

	public async ValueTask<bool> Get(T parameter)
	{
		using var session = _authentications.Get();
		var (authentication, users) = session;
		var user = await users.FindByIdAsync(parameter.Id.ToString()).ConfigureAwait(false);
		var result = await authentication.ValidateSecurityStampAsync(user, parameter.SecurityStamp)
		                                 .ConfigureAwait(false);
		return result;
	}
}