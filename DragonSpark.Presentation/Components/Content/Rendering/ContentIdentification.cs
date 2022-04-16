using DragonSpark.Application.Navigation;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentIdentification : IFormatter<object>
{
	readonly CurrentPath                _path;
	readonly IResult<ContentIdentifier> _identifiers;

	public ContentIdentification(CurrentPath path)
		: this(path, ContentIdentifiers.Default.Then().Bind((object)path).Get()) {}

	public ContentIdentification(CurrentPath path, IResult<ContentIdentifier> identifiers)
	{
		_path        = path;
		_identifiers = identifiers;
	}

	public string Get(object parameter)
	{
		var result = $"{_path.Get()}+{_identifiers.Get().Get(parameter)}";
		return result;
	}
}