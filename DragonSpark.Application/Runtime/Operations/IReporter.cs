using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Runtime.Operations;

public interface IReporter<T> : IReporter<T, T> {}

public interface IReporter<TIn, out TOut> : ISelect<Report<TIn>, TOut> {}