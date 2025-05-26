using DragonSpark.Application.AspNet.Entities.Queries.Compiled;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities;

sealed class Input<TIn, T, TOut> : StopAwareInline<In<TIn>, IAsyncEnumerable<T>, TOut>, IInput<TIn, TOut>
{
	public Input(IElements<TIn, T> elements, IEvaluate<T, TOut> evaluate) : base(elements, evaluate) {}
}