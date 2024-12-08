using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled;

public interface IReading<in TIn, T> : ISelect<TIn, Reading<T>>;