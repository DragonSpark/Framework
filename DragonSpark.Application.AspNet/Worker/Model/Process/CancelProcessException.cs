namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public sealed class CancelProcessException : AbortProcessException
{
	public CancelProcessException(string reason) : base(reason, $"A cancellation request was detected: {reason}") {}
}