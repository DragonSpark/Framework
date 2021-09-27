using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Editing
{
	class Class4 {}

	public interface IForming<TIn, T> : ISelecting<In<TIn>, T> {}

	sealed class Forming<TIn, T, TOut> : Selecting<In<TIn>, IAsyncEnumerable<T>, TOut>, IForming<TIn, TOut>
	{
		public Forming(IElements<TIn, T> elements, IEvaluate<T, TOut> evaluate)
			: base(elements.Then().Operation(), evaluate.Get) {}
	}
}