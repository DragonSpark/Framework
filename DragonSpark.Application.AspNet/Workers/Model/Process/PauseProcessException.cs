namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public sealed class PauseProcessException : AbortProcessException
{
	public PauseProcessException(string reason) : base(reason, $"A process was paused: {reason}") {}
}