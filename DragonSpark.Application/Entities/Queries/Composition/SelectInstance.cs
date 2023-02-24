using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class SelectInstance<T, TTo> : Start<T, TTo> where T : class
{
	protected SelectInstance(IInstance<TTo> instance) : this(instance.Get().Then().Elide().Get()) {}

	protected SelectInstance(Expression<Func<DbContext, TTo>> general)
		: base((d, q) => q.AsSplitQuery().Select(_ => general.Invoke(d))) {}
}