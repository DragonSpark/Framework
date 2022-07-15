using DragonSpark.Presentation.Connections;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderContentKey : IRenderContentKey
{
	readonly IConnectionIdentifier _identifier;
	readonly IContentKey           _key;

	public RenderContentKey(IConnectionIdentifier identifier, IContentKey key)
	{
		_identifier = identifier;
		_key        = key;
	}

	public string Get(object parameter) => $"{_identifier.Get().ToString()}/{_key.Get(parameter)}";
}