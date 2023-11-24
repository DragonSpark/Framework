namespace DragonSpark.Diagnostics;

public sealed class LinearRetryTime : RetryTimeBase
{
	public static LinearRetryTime Default { get; } = new();

	LinearRetryTime() : base(parameter => parameter) {}
}