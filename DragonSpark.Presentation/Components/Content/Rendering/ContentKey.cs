using DragonSpark.Application.Security.Identity;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentKey : IContentKey
{
	readonly ICurrentUserName      _name;
	readonly ContentIdentification _content;

	public ContentKey(ICurrentUserName name, ContentIdentification content)
	{
		_name    = name;
		_content = content;
	}

	public string Get(object parameter) => $"{_name.Get()}/{_content.Get(parameter)}";
}