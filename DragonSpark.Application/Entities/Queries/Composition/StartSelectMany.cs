using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartSelectMany<TFrom, TTo> : SelectMany<TFrom, TTo> where TFrom : class
{
	protected StartSelectMany(Expression<Func<TFrom, IEnumerable<TTo>>> select) : this(x => x, select) {}

	protected StartSelectMany(Expression<Func<DbContext, TFrom, IEnumerable<TTo>>> select)
		: base((x, _) => x.Set<TFrom>(), (d, _, x) => select.Invoke(d, x)) {}

	protected StartSelectMany(Expression<Func<IQueryable<TFrom>, IQueryable<TFrom>>> previous,
	                          Expression<Func<TFrom, IEnumerable<TTo>>> select)
		: base(context => previous.Invoke(context.Set<TFrom>()), select) {}
}