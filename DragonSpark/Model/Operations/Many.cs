using DragonSpark.Model.Sequences;

namespace DragonSpark.Model.Operations;

public readonly record struct Many<T>(Array<IAltering<T>> Alterations, T Seed);