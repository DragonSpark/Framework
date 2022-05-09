using DragonSpark.Model.Sequences;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class OrderActions : Instances<string>
{
	public static OrderActions Default { get; } = new();

	OrderActions() : base("MoveUp", "MoveDown") {}
}