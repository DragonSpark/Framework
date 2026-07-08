using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Contracts.Worker;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public readonly record struct UpdateProcessStatusInput(ExternalProcess Process, ProcessStatus Status, string Message);