namespace DragonSpark.Server.Output;

public readonly record struct ProcessTagsInput(object Subject, List<string> Tags);