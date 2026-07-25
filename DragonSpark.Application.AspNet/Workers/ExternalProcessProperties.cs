using DragonSpark.Contracts.Worker;

namespace DragonSpark.Application.AspNet.Workers;

public sealed record ExternalProcessProperties(Guid Id, DateTimeOffset Created, ProcessStatus Status, string Message);