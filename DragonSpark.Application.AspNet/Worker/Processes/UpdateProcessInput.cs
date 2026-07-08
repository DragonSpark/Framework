namespace DragonSpark.Application.AspNet.Worker.Processes;

public readonly record struct UpdateProcessInput(ExternalProcess Process, ProcessUpdate Update);