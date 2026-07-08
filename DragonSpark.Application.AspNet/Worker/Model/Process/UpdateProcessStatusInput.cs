using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Contracts.Worker;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public readonly record struct UpdateProcessStatusInput(ExternalProcess Process, ProcessStatus Status, string Message);