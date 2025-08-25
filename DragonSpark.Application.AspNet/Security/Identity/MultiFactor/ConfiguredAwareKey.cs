using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.MultiFactor;

sealed class ConfiguredAwareKey<T> : ISelecting<Input<T>, string> where T : IdentityUser
{
	public static ConfiguredAwareKey<T> Default { get; } = new();

	ConfiguredAwareKey() : this(Key<T>.Default) {}

	readonly ISelecting<Input<T>, string?> _previous;

	public ConfiguredAwareKey(ISelecting<Input<T>, string?> previous) => _previous = previous;

	public async ValueTask<string> Get(Input<T> parameter)
	{
		var previous = await _previous.Off(parameter);
		if (string.IsNullOrEmpty(previous))
		{
			var (manager, user) = parameter;
			var updated = await manager.FindByIdAsync(await manager.GetUserIdAsync(user).Off()).Off();
			await manager.ResetAuthenticatorKeyAsync(updated.Verify()).Off();
			var result = await _previous.Off(parameter);
			return result.Verify();
		}

		return previous;
	}
}