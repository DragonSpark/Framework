using DragonSpark.Application.Runtime.Operations;

namespace DragonSpark.Presentation.Components.State;

public sealed record ActivityOptions(
	string? Message = null,
	ITokenHandle? Handle = null,
	bool RenderOnCompletion = true)
{
	public static ActivityOptions Default { get; } = new();
	public static ActivityOptions NoPostRender { get; } = new(RenderOnCompletion: false);
}