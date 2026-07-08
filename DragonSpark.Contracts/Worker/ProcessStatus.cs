namespace DragonSpark.Contracts.Worker;

public enum ProcessStatus : byte
{
	New, Queued, Processing, Completed, Error, Canceled, Paused
}