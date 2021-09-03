using DragonSpark.Application.Entities.Queries.Evaluation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	public interface IFormed<TIn, T> : ISelecting<In<TIn>, T> {}

	public class Formed<TIn, T> : Selecting<In<TIn>, T>, IFormed<TIn, T>
	{
		protected Formed(ISelect<In<TIn>, ValueTask<T>> @select) : base(@select) {}

		protected Formed(Func<In<TIn>, ValueTask<T>> @select) : base(@select) {}
	}

	sealed class Formed<TIn, T, TOut> : Selecting<In<TIn>, IAsyncEnumerable<T>, TOut>, IFormed<TIn, TOut>
	{
		public Formed(IForm<TIn, T> form, IEvaluate<T, TOut> evaluate) : base(form.Then().Operation(), evaluate.Get) {}
	}
}