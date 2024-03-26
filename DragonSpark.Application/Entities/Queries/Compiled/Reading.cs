using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

public readonly record struct Reading<T>(DbContext Context, IDisposable Disposable, IAsyncEnumerable<T> Elements) : IDisposable
{
	public void Dispose()
	{
		Disposable.Dispose();
	}
}

public sealed class Reading<TIn, T> : IReading<TIn, T>
{
	readonly IContexts         _contexts;
	readonly IElements<TIn, T> _elements;

	public Reading(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(contexts, new Elements<TIn, T>(expression)) {}

	public Reading(IContexts contexts, IElements<TIn, T> elements)
	{
		_contexts = contexts;
		_elements = elements;
	}

	public Reading<T> Get(TIn parameter)
	{
		var (context, disposable) = _contexts.Get();
		var elements = _elements.Get(new(context, parameter));
		return new(context, disposable, elements);
	}
}