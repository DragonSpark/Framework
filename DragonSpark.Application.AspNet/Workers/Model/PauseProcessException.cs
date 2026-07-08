namespace DragonSpark.Application.AspNet.Workers.Model;

public sealed class PauseProcessException : AbortProcessException
{
	public PauseProcessException(string reason) : base(reason, $"A process was paused: {reason}") {}
}