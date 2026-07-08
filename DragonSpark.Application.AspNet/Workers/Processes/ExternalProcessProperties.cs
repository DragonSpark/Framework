using DragonSpark.Contracts.Worker;
using System;

namespace DragonSpark.Application.AspNet.Workers.Processes;

public sealed record ExternalProcessProperties(Guid Id, DateTimeOffset Created, ProcessStatus Status, string Message);