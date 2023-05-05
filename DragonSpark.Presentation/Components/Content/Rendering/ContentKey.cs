using DragonSpark.Application.Security.Identity;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentKey : IContentKey
{
	readonly ICurrentUserNumber    _number;
	readonly ContentIdentification _content;

	public ContentKey(ICurrentUserNumber number, ContentIdentification content)
	{
		_number  = number;
		_content = content;
	}

	public string Get(object parameter) => $"{_number.Get()}/{_content.Get(parameter)}";
}