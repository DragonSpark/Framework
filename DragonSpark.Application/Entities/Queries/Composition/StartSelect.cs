using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartSelect<TFrom, TTo> : Select<TFrom, TTo> where TFrom : class
{
	protected StartSelect(Expression<Func<TFrom, TTo>> select) : base(Set<TFrom>.Default.Then(), select) {}

	protected StartSelect(Expression<Func<IQueryable<TFrom>, IQueryable<TFrom>>> start,
	                      Expression<Func<TFrom, TTo>> select)
		: base(context => start.Invoke(context.Set<TFrom>()), select) {}
}

public class StartSelect<TIn, TFrom, TTo> : Select<TIn, TFrom, TTo> where TFrom : class
{
	protected StartSelect(Expression<Func<TFrom, TTo>> select) : base(Set<TIn, TFrom>.Default.Then(), select) {}
}