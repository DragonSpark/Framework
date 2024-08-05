using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components;

public readonly record struct ElementIdentifier(string Name)
{
	public static implicit operator ElementIdentifier(string instance) => new(instance);

	public override string ToString() => $"#{Name}";
}

// TODO

public sealed class EmptyCallback : Select<object, EventCallback>
{
	public static EmptyCallback Default { get; } = new();

	EmptyCallback() : base(x => EventCallback.Factory.Create(x, () => {})) {}
}