using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Queries.Composition;

public class StartSelect<TFrom, TTo> : Select<TFrom, TTo> where TFrom : class
{
	protected StartSelect(Expression<Func<TFrom, TTo>> select) : base(Set<TFrom>.Default.Then(), select) {}

	protected StartSelect(Expression<Func<IQueryable<TFrom>, IQueryable<TFrom>>> start,
	                      Expression<Func<TFrom, TTo>> select)
		: base(context => start.Invoke(context.Set<TFrom>()), select) {}

	protected StartSelect(Expression<Func<IQueryable<TFrom>, IQueryable<TFrom>>> start,
	                      Expression<Func<DbContext, TFrom, TTo>> select)
		: base(context => start.Invoke(context.Set<TFrom>()), select) {}
}

public class StartSelect<TIn, TFrom, TTo> : Select<TIn, TFrom, TTo> where TFrom : class
{
	public StartSelect(Expression<Func<TFrom, TTo>> select) : this(Set<TFrom>.Default.Then().Accept<TIn>(), select) {}

	public StartSelect(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous,
	                   Expression<Func<TFrom, TTo>> select)
		: base(previous, select) {}

	public StartSelect(Expression<Func<TIn, TFrom, TTo>> select)
		: this(Set<TFrom>.Default.Then().Accept<TIn>(), select) {}

	public StartSelect(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous,
	                   Expression<Func<TIn, TFrom, TTo>> select)
		: base(previous, select) {}

	public StartSelect(Expression<Func<DbContext, TIn, TFrom, TTo>> select)
		: this(Set<TFrom>.Default.Then().Accept<TIn>(), select) {}

	public StartSelect(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous,
	                   Expression<Func<DbContext, TIn, TFrom, TTo>> select) : base(previous, select) {}
}