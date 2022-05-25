using DragonSpark.Text;
using System;

namespace DragonSpark.Presentation.Components;

sealed class GetElementIdentifier : IFormatter<Guid>
{
	public static GetElementIdentifier Default { get; } = new();

	GetElementIdentifier() : this(UniqueIdentifier.Default) {}

	readonly IFormatter<Guid> _previous;

	public GetElementIdentifier(IFormatter<Guid> previous) => _previous = previous;

	public string Get(Guid parameter) => $"gid_{_previous.Get(parameter)}";
}