using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

sealed class Key<T> : ISelecting<UserInput<T>, string> where T : IdentityUser
{
	public static Key<T> Default { get; } = new();

	Key() {}

	public async ValueTask<string> Get(UserInput<T> parameter)
	{
		var (manager, user) = parameter;
		return await manager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
	}
}