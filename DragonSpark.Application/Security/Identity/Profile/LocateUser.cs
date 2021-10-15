using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class LocateUser<T> : ILocateUser<T> where T : IdentityUser
{
	readonly IUsers<T> _users;

	public LocateUser(IUsers<T> users) => _users = users;

	public async ValueTask<T?> Get(ExternalLoginInfo parameter)
	{
		using var users = _users.Get();
		var result = await users.Subject.FindByLoginAsync(parameter.LoginProvider, parameter.ProviderKey)
		                        .ConfigureAwait(false);
		return result;
	}
}