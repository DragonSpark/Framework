using DragonSpark.Model.Sequences;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Environment.Browser.Window;

public readonly record struct NewWindowFocusElementInput(object Reference) : IArray<object>
{
	public Array<object> Get() => new object[] { DotNetObjectReference.Create(Reference) };
}