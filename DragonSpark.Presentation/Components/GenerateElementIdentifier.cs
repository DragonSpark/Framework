using DragonSpark.Text;
using System;

namespace DragonSpark.Presentation.Components;

public sealed class GenerateElementIdentifier : IFormatter<Guid>
{
	public static GenerateElementIdentifier Default { get; } = new();

	GenerateElementIdentifier() : this(UniqueIdentifier.Default) {}

	readonly IFormatter<Guid> _previous;

	public GenerateElementIdentifier(IFormatter<Guid> previous) => _previous = previous;

	public string Get(Guid parameter) => $"gid_{_previous.Get(parameter)}";
}