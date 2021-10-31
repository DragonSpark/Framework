using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

sealed class ConfiguredAwareKey<T> : IKey<T> where T : IdentityUser
{
	public static ConfiguredAwareKey<T> Default { get; } = new();

	ConfiguredAwareKey() : this(Key<T>.Default) {}

	readonly IKey<T> _previous;

	public ConfiguredAwareKey(IKey<T> previous) => _previous = previous;

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