namespace DragonSpark.Contracts.Queries;

public readonly record struct PageQueryInput<T>(T Parameter, PageInput Input);