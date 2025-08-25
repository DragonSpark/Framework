using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.MultiFactor;

sealed class KeyCode<T> : IKeyCode<T> where T : IdentityUser
{
	readonly KeyApplicationLocation           _location;
	readonly ISelecting<UserInput<T>, string> _key;

	public KeyCode(KeyApplicationLocation location) : this(location, ConfiguredAwareKey<T>.Default) {}

	public KeyCode(KeyApplicationLocation location, ISelecting<UserInput<T>, string> key)
	{
		_location = location;
		_key      = key;
	}

	public async ValueTask<KeyCodeView> Get(UserInput<T> parameter)
	{
		var (manager, user) = parameter;
		var key    = await _key.Off(parameter);
		var result = _location.Get(new(await manager.GetUserIdAsync(user).Off(), key));
		return new(key, result);
	}
}
