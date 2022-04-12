using Syncfusion.Blazor.Navigations;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class DefaultAnimationSettings : TabAnimationSettings
{
	public static DefaultAnimationSettings Default { get; } = new();

	DefaultAnimationSettings()
	{
		Previous = DefaultPreviousAnimation.Default;
		Next     = DefaultNextAnimation.Default;
	}
}