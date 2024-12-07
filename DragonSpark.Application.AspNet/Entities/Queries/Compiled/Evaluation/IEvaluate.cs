using DragonSpark.Model.Operations.Selection;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public interface IEvaluate<in T, TResult> : ISelecting<IAsyncEnumerable<T>, TResult>;