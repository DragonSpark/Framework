using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Invocation;

public sealed class MaximumParallelism : Instance<ushort>
{
	public static MaximumParallelism Default { get; } = new();

	MaximumParallelism() : base((ushort)System.Environment.ProcessorCount) {}
}