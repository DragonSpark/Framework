using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Entities.Queries.Compiled;

public interface IReading<in TIn, T> : ISelecting<TIn, Reading<T>>;