using Syncfusion.Blazor;
using Syncfusion.Blazor.Navigations;

namespace DragonSpark.SyncfusionRendering.Components;

sealed class DefaultNextAnimation : TabAnimationNext
{
	public static DefaultNextAnimation Default { get; } = new();

	DefaultNextAnimation()
	{
		Effect   = AnimationEffect.None;
		Duration = 0;
	}
}