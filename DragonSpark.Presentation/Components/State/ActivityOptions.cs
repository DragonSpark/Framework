using DragonSpark.Application.Runtime.Operations;
using System.Numerics;

namespace DragonSpark.Presentation.Components.State;

public readonly record struct ActivityOptions(
	string? Message = null,
	ITokenHandle? Handle = null,
	bool RenderOnCompletion = true)
	: IEqualityOperators<ActivityOptions, ActivityOptions, bool>
{
	public static ActivityOptions Default { get; } = new();
	public static ActivityOptions NoRender { get; } = new(RenderOnCompletion: false);
}