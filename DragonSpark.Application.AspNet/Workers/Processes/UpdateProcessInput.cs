namespace DragonSpark.Application.AspNet.Workers.Processes;

public readonly record struct UpdateProcessInput(ExternalProcess Process, ProcessUpdate Update);