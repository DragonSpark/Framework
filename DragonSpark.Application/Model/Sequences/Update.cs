namespace DragonSpark.Application.Model.Sequences;

public readonly record struct Update<T>(T Stored, T Input);

public readonly record struct Update<TModel, TView>(TModel Stored, TView Input);