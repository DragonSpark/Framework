using DragonSpark.Compose;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering;

public sealed class SyncfusionVersion : Text.Text
{
	public static SyncfusionVersion Default { get; } = new();

	SyncfusionVersion() : base(typeof(SfBaseComponent).Assembly.GetName().Version.Verify().ToString(3)) {}
}