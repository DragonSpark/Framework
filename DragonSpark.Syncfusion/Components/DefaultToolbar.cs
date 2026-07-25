using DragonSpark.Compose;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class DefaultToolbar : List<string>
{
	public static DefaultToolbar Default { get; } = new();

	DefaultToolbar() : base("Filter".Yield()) {}
}