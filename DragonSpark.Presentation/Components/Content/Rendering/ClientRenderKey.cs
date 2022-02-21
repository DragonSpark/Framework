using DragonSpark.Application.Components;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

public class ClientRenderKey : IResult<string>
{
	readonly IClientIdentifier _identifier;
	readonly string            _qualifier;

	protected ClientRenderKey(IClientIdentifier identifier, Type qualifier)
		: this(identifier, qualifier.AssemblyQualifiedName.Verify()) {}

	protected ClientRenderKey(IClientIdentifier identifier, string qualifier)
	{
		_identifier = identifier;
		_qualifier  = qualifier;
	}

	public string Get() => $"{_identifier.Get().ToString()}+{_qualifier}";
}