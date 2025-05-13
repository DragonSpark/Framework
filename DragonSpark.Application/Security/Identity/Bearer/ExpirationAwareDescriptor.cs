using DragonSpark.Application.Diagnostics.Time;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.Security.Identity.Bearer;

public class ExpirationAwareDescriptor<T> : ISelect<T, SecurityTokenDescriptor> where T : class
{
	readonly ITable<T, SecurityTokenDescriptor> _source;
	readonly IWindow                            _window;

	protected ExpirationAwareDescriptor(ISelect<T, SecurityTokenDescriptor> previous, BearerSettings settings)
		: this(previous.Then().Stores().New(), Time.Default.FromThen(settings.Window)) {}

	protected ExpirationAwareDescriptor(ITable<T, SecurityTokenDescriptor> source, IWindow window)
	{
		_source = source;
		_window = window;
	}

	public SecurityTokenDescriptor Get(T parameter)
	{
		var stored = _source.Get(parameter);
		var expired = stored.Expires.HasValue
		              &&
		              _window.Get(stored.Expires.GetValueOrDefault())
		              &&
		              _source.Remove(parameter);
		var result = expired ? _source.Get(parameter) : stored;
		return result;
	}
}