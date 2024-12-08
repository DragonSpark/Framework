using DragonSpark.Compose;
using JetBrains.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.State;

sealed class StateUser<T> : IStateUser<T> where T : IdentityUser
{
	readonly IUsers<T> _users;

	[UsedImplicitly]
	public StateUser(IUsers<T> users) => _users = users;

	public async ValueTask<T?> Get(ClaimsPrincipal parameter)
	{
		using var users  = _users.Get();
		var       result = await users.Subject.GetUserAsync(parameter).Await();
		return result;
	}
}