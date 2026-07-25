using DragonSpark.Application.AspNet.Navigation;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentIdentification : IFormatter<ContentKeyInput>
{
	readonly CurrentPath       _path;
	readonly ContentIdentifier _identifiers;

	public ContentIdentification(CurrentPath path, ContentIdentifier identifiers)
	{
		_path        = path;
		_identifiers = identifiers;
	}

	public string Get(ContentKeyInput parameter) => $"{_path.Get()}+{_identifiers.Get(parameter)}";
}