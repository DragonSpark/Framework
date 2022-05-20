using DragonSpark.Diagnostics;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class IsInteropException : HasMessage
{
	public static IsInteropException Default { get; } = new();

	IsInteropException()
		: base("JavaScript interop calls cannot be issued at this time. This is because the component is being statically rendered. When prerendering is enabled, JavaScript interop calls can only be performed during the OnAfterRenderAsync lifecycle method") {}
}