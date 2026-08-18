using DragonSpark.Model.Results;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public sealed class DefaultMaximumUploadSize : Instance<uint>
{
	public static DefaultMaximumUploadSize Default { get; } = new();

	DefaultMaximumUploadSize() : base(128 * 1024 * 1024) {}
}