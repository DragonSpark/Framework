using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities;

sealed class Input<TIn, T, TOut> : Selecting<In<TIn>, IAsyncEnumerable<T>, TOut>, IInput<TIn, TOut>
{
	public Input(IElements<TIn, T> elements, IEvaluate<T, TOut> evaluate)
		: base(elements.Then().Operation(), evaluate.Get) {}
}