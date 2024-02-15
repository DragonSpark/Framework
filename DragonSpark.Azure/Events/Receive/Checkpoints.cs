using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Events.Receive;

sealed class Checkpoints : ConcurrentTable<string, ProcessCheckpoint>
{
	public static Checkpoints Default { get; } = new();

	Checkpoints() : base(_ => new()) {}
}