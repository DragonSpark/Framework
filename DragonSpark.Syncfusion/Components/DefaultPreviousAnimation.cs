using Syncfusion.Blazor;
using Syncfusion.Blazor.Navigations;

namespace DragonSpark.SyncfusionRendering.Components;

sealed class DefaultPreviousAnimation : TabAnimationPrevious
{
	public static DefaultPreviousAnimation Default { get; } = new();

	DefaultPreviousAnimation()
	{
		Effect   = AnimationEffect.None;
		Duration = 0;
	}
}