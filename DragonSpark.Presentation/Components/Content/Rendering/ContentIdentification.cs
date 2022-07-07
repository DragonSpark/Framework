using DragonSpark.Application.Navigation;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentIdentification : IFormatter<object>
{
	readonly CurrentPath       _path;
	readonly ContentIdentifier _identifiers;

	public ContentIdentification(CurrentPath path, ContentIdentifier identifiers)
	{
		_path        = path;
		_identifiers = identifiers;
	}

	public string Get(object parameter)
	{
		var result = $"{_path.Get()}+{_identifiers.Get(parameter)}";
		return result;
	}
}