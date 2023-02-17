using DragonSpark.Model.Sequences;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Environment.Browser.Window;

sealed class NewWindowFocusElement : CreateReference<NewWindowFocusElementInput>
{
	public static NewWindowFocusElement Default { get; } = new();

	NewWindowFocusElement() : base(nameof(NewWindowFocusElement)) {}
}

public readonly record struct NewWindowFocusElementInput(object Reference) : IArray<object>
{
	public Array<object> Get() => new object[] { DotNetObjectReference.Create(Reference) };
}