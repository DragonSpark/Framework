using DragonSpark.Model.Results;
using System.Numerics;

namespace DragonSpark.Runtime.Invocation;

public sealed class MaximumParallelism : Instance<ushort>
{
	public static MaximumParallelism Default { get; } = new();

	MaximumParallelism() : base((ushort)System.Environment.ProcessorCount) {}
}

public sealed class MaximumParallelismSafe : Instance<ushort>
{
	public static MaximumParallelismSafe Default { get; } = new();

	MaximumParallelismSafe() : this(MaximumParallelism.Default) {}

	public MaximumParallelismSafe(ushort previous)
		: base((ushort)BitOperations.RoundUpToPowerOf2(Math.Max((ushort)1, previous))) {}
}