using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Security.Identity.MultiFactor;

sealed class KeyCode<T>(KeyApplicationLocation location, ISelecting<UserInput<T>, string> key)
	: IKeyCode<T> where T : IdentityUser
{
	public static KeyCode<T> Default { get; } = new();

	KeyCode() : this(KeyApplicationLocation.Default, ConfiguredAwareKey<T>.Default) {}

	readonly KeyApplicationLocation           _location = location;
	readonly ISelecting<UserInput<T>, string> _key      = key;

	public async ValueTask<KeyCodeView> Get(UserInput<T> parameter)
	{
		var (manager, user) = parameter;
		var key    = await _key.Off(parameter);
		var result = _location.Get(new(await manager.GetUserIdAsync(user).Off(), key));
		return new(key, result);
	}
}
