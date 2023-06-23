using DragonSpark.Model.Sequences;

namespace DragonSpark.Model.Operations.Selection;

public readonly record struct Many<T>(Array<IAltering<T>> Alterations, T Seed);