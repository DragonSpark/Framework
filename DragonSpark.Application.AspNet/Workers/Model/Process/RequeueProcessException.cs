namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public class RequeueProcessException : AbortProcessException
{
	public RequeueProcessException(string reason)
		: base(reason, $"A request to requeue this process was detected: {reason}") {}
}