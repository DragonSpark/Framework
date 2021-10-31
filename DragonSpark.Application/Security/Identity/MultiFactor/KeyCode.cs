using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

sealed class KeyCode<T> : IKeyCode<T> where T : IdentityUser
{
	public static KeyCode<T> Default { get; } = new();

	KeyCode() : this(KeyApplicationLocation.Default, ConfiguredAwareKey<T>.Default) {}

	readonly KeyApplicationLocation _location;
	readonly IKey<T>                _key;

	public KeyCode(KeyApplicationLocation location, IKey<T> key)
	{
		_location = location;
		_key      = key;
	}

	public async ValueTask<KeyCodeView> Get(UserInput<T> parameter)
	{
		var (manager, user) = parameter;
		var key    = await _key.Await(parameter);
		var result = _location.Get(new(await manager.GetUserIdAsync(user), key));
		return new(key, result);
	}
}