using JetBrains.Annotations;

namespace DragonSpark.Diagnostics;

public sealed class LinearRetryTime : RetryTimeBase
{
	[UsedImplicitly]
	public static LinearRetryTime Default { get; } = new();

	LinearRetryTime() : base(x => x) {}
}