namespace DragonSpark.Server.Output;

public readonly record struct ComposeTagsInput(object Parameter, IOutputKey Key, List<string> Result);