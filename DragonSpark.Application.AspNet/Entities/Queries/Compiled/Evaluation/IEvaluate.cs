using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public interface IEvaluate<T, TResult> : IStopAware<IAsyncEnumerable<T>, TResult>;