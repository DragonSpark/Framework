using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

sealed class Create<T> : ICreate<T> where T : class
{
	readonly IUsers<T> _users;

	public Create(IUsers<T> users) => _users = users;

	public async ValueTask<IdentityResult> Get(Login<T> parameter)
	{
		var (_, user) = parameter;
		using var users  = _users.Get();
		var       result = await users.Subject.CreateAsync(user).Await();
		return result;
	}
}