namespace DragonSpark.Server.Output;

public readonly record struct TagDefinitionInput<TIn, TOut>(TIn Input, TOut? Output);