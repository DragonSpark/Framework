using DragonSpark.Model.Sequences;

namespace DragonSpark.SyncfusionRendering.Components;

internal class Class1 {}

public sealed class OrderActions : Instances<string>
{
	public static OrderActions Default { get; } = new();

	OrderActions() : base("MoveUp", "MoveDown") {}
}