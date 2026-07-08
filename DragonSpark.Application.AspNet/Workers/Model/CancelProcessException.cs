namespace DragonSpark.Application.AspNet.Workers.Model;

public sealed class CancelProcessException : AbortProcessException
{
	public CancelProcessException(string reason) : base(reason, $"A cancellation request was detected: {reason}") {}
}