namespace DragonSpark.Application.AspNet.Workers;

public readonly record struct UpdateProcessInput(ExternalProcess Process, ProcessUpdate Update);