using DragonSpark.Application.Components;
using DragonSpark.Compose;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderContentKey : IRenderContentKey
{
	readonly IClientIdentifier _identifier;

	public RenderContentKey(IClientIdentifier identifier) => _identifier = identifier;

	public string Get(Delegate parameter)
		=> $"{_identifier.Get().ToString()}+{parameter.Method.DeclaringType.Verify().AssemblyQualifiedName}+{parameter.Method.Name}";
}