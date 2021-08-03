using DragonSpark.Application.Model;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose
{
	public sealed class ProjectionContext<_, TOut> : Selector<_, IQueryable<TOut>>
	{
		public ProjectionContext(ISelect<_, IQueryable<TOut>> subject) : base(subject) {}

		public ProjectionContext<_, T> Select<T>(Expression<Func<TOut, T>> selection)
			=> new ProjectionContext<_, T>(Get().Select(new Projection<TOut, T>(selection)));
	}
}