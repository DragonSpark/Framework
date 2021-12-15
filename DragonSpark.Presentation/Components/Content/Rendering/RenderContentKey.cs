using DragonSpark.Application.Components;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderContentKey : IRenderContentKey
{
	readonly IClientIdentifier     _identifier;
	readonly ContentIdentification _content;

	public RenderContentKey(IClientIdentifier identifier, ContentIdentification content)
	{
		_identifier   = identifier;
		_content = content;
	}

	public string Get(object parameter) => $"{_identifier.Get().ToString()}+{_content.Get(parameter)}";
}