using DragonSpark.Model.Results;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public sealed class Attributes : Instance<Dictionary<string, object>>
{
	public static Attributes Default { get; } = new();

	Attributes() : base(new() { { "name", "files" } }) {}
}