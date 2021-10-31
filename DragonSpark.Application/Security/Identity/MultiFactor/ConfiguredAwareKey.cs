using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

sealed class ConfiguredAwareKey<T> : ISelecting<UserInput<T>, string> where T : IdentityUser
{
	public static ConfiguredAwareKey<T> Default { get; } = new();

	ConfiguredAwareKey() : this(Key<T>.Default) {}

	readonly ISelecting<UserInput<T>, string> _previous;

	public ConfiguredAwareKey(ISelecting<UserInput<T>, string> previous) => _previous = previous;

	public async ValueTask<string> Get(UserInput<T> parameter)
	{
		var previous = await _previous.Await(parameter);
		if (string.IsNullOrEmpty(previous))
		{
			var (manager, user) = parameter;
			var updated = await manager.FindByIdAsync(await manager.GetUserIdAsync(user).ConfigureAwait(false))
			                           .ConfigureAwait(false);
			await manager.ResetAuthenticatorKeyAsync(updated).ConfigureAwait(false);
			return await _previous.Await(parameter);
		}

		return previous;
	}
}