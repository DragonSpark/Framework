using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Composition;

public class SelectInstance<T, TTo> : Start<T, TTo> where T : class
{
	public SelectInstance(IInstance<TTo> instance) : this(instance.Get().Then().Elide().Get()) {}

	protected SelectInstance(Expression<Func<DbContext, TTo>> general)
		: base((d, q) => q.AsSplitQuery().Select(_ => general.Invoke(d))) {}
}

public class SelectInstance<TIn, T, TTo> : StartInput<TIn, T, TTo> where T : class
{
	public SelectInstance(IInstance<TIn, TTo> instance) : this(instance.Get()) {}

	protected SelectInstance(Expression<Func<DbContext, TIn, TTo>> general)
		: base((d, p, q) => q.AsSplitQuery().Select(_ => general.Invoke(d, p))) {}
}