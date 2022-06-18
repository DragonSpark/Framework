using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Rendering;

public class RenderContentSubKey : IFormatter<object>
{
	readonly IRenderContentKey _source;
	readonly string            _key;

	protected RenderContentSubKey(IRenderContentKey source, string key)
	{
		_source = source;
		_key    = key;
	}

	public string Get(object parameter) => $"{_source.Get(parameter)}/{_key}";
}