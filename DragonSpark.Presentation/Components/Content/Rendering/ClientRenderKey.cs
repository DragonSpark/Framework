using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Connections;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

public class ClientRenderKey : IResult<string>
{
	readonly IConnectionIdentifier _identifier;
	readonly string                          _qualifier;

	protected ClientRenderKey(IConnectionIdentifier identifier, Type qualifier)
		: this(identifier, qualifier.AssemblyQualifiedName.Verify()) {}

	protected ClientRenderKey(IConnectionIdentifier identifier, string qualifier)
	{
		_identifier = identifier;
		_qualifier  = qualifier;
	}

	public string Get() => $"{_identifier.Get().ToString()}+{_qualifier}";
}