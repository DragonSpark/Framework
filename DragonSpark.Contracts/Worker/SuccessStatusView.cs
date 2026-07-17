namespace DragonSpark.Contracts.Worker;

public sealed record SuccessStatusView(Guid Identity, ProcessStatus Status, DateTimeOffset Time, string? Message)
	: ProcessStatusView(Status, Time, Message);