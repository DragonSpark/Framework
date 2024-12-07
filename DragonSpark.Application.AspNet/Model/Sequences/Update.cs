namespace DragonSpark.Application.Model.Sequences;

public readonly record struct Update<T>(T Stored, T Input);

public readonly record struct Update<TView, TModel>(TView View, TModel Model);