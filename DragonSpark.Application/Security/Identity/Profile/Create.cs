using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class Create<T> : ICreate<T> where T : class
{
	readonly IUsers<T> _users;

	public Create(IUsers<T> users) => _users = users;

	public async ValueTask<IdentityResult> Get(Login<T> parameter)
	{
		using var users  = _users.Get();
		var       result = await users.Subject.CreateAsync(parameter.User).ConfigureAwait(false);
		return result;
	}
}