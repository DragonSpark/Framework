using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

sealed class FormatAwareKeyCode<T> : IKeyCode<T> where T : class
{
	readonly IKeyCode<T> _previous;
	readonly FormatKey   _format;

	public FormatAwareKeyCode(IKeyCode<T> previous) : this(previous, FormatKey.Default) {}

	public FormatAwareKeyCode(IKeyCode<T> previous, FormatKey format)
	{
		_previous = previous;
		_format   = format;
	}

	public async ValueTask<KeyCodeView> Get(UserInput<T> parameter)
	{
		var previous = await _previous.Await(parameter);
		var result   = previous with { Key = _format.Get(previous.Key) };
		return result;
	}
}