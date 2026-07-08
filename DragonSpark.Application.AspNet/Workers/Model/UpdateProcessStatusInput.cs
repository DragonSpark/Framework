using DragonSpark.Contracts.Worker;

namespace DragonSpark.Application.AspNet.Workers.Model;

public readonly record struct UpdateProcessStatusInput(ExternalProcess Process, ProcessStatus Status, string Message);