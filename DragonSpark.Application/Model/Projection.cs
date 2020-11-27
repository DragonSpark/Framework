using DragonSpark.Model.Selection;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Model
{
	sealed class Projection<TIn, TOut> : ISelect<IQueryable<TIn>, IQueryable<TOut>>
	{
		readonly Expression<Func<TIn, TOut>> _selection;

		public Projection(Expression<Func<TIn, TOut>> selection) => _selection = selection;

		public IQueryable<TOut> Get(IQueryable<TIn> parameter) => parameter.Select(_selection);
	}
}