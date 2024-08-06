using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components;

public sealed class EmptyCallback : Select<object, EventCallback>
{
	public static EmptyCallback Default { get; } = new();

	EmptyCallback() : base(x => EventCallback.Factory.Create(x, () => {})) {}
}