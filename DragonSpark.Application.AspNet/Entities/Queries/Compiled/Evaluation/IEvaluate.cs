using DragonSpark.Model.Operations.Selection.Stop;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public interface IEvaluate<T, TResult> : IStopAware<IAsyncEnumerable<T>, TResult>;