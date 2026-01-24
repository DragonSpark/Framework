namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct MapInput<TFrom, TTo>(Entry<TFrom> From, Entry<TTo> To);