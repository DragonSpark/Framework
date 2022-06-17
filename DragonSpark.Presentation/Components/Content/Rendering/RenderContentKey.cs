using DragonSpark.Application.Security.Identity;
using DragonSpark.Presentation.Connections;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderContentKey : IRenderContentKey
{
	readonly IConnectionIdentifier _identifier;
	readonly ICurrentUserName      _name;
	readonly ContentIdentification _content;

	public RenderContentKey(IConnectionIdentifier identifier, ICurrentUserName name, ContentIdentification content)
	{
		_identifier = identifier;
		_name       = name;
		_content    = content;
	}

	public string Get(object parameter) => $"{_identifier.Get().ToString()}/{_name.Get()}/{_content.Get(parameter)}";
}