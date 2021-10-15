using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class Start<T> : Combine<T> where T : class
{
	protected Start(Expression<Func<IQueryable<T>, IQueryable<T>>> select)
		: base(Set<T>.Default.Get(), select) {}

	protected Start(Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select)
		: base(Set<T>.Default.Get(), select) {}
}

public class Start<T, TTo> : Combine<T, TTo> where T : class
{
	protected Start(Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
		: base(Set<T>.Default.Get(), select) {}

	protected Start(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> select)
		: base(Set<T>.Default.Get(), select) {}
}