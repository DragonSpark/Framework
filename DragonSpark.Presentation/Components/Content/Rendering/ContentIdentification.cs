using DragonSpark.Application.Navigation;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Security;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentIdentification : IFormatter<Microsoft.AspNetCore.Components.ComponentBase>
{
	readonly CurrentPath                _path;
	readonly IResult<ContentIdentifier> _identifiers;

	public ContentIdentification(CurrentPath path, ICurrentContext context)
		: this(path, ContentIdentifiers.Default.Then().Bind(context).Get()) {}

	public ContentIdentification(CurrentPath path, IResult<ContentIdentifier> identifiers)
	{
		_path        = path;
		_identifiers = identifiers;
	}

	public string Get(Microsoft.AspNetCore.Components.ComponentBase parameter)
	{
		var result = $"{_path.Get()}+{_identifiers.Get().Get(parameter)}";
		return result;
	}
}