namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public sealed class PauseProcessException : AbortProcessException
{
	public PauseProcessException(string reason) : base(reason, $"A process was paused: {reason}") {}
}