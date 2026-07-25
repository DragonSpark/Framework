namespace DragonSpark.Application.Runtime.Operations;

public readonly record struct WorkerInput(Task Subject, Action Complete);