namespace DragonSpark.Diagnostics;

public sealed class LinearRetryTime : RetryTimeBase
{
	public static LinearRetryTime Default { get; } = new LinearRetryTime();

	LinearRetryTime() : base(parameter => parameter) {}
}