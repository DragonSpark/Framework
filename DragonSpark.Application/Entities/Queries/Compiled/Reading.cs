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
	readonly IScopes         _scopes;
	readonly IElements<TIn, T> _elements;

	public Reading(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(scopes, new Elements<TIn, T>(expression)) {}

	public Reading(IScopes scopes, IElements<TIn, T> elements)
	{
		_scopes = scopes;
		_elements = elements;
	}

	public Reading<T> Get(TIn parameter)
	{
		var (context, disposable) = _scopes.Get();
		var elements = _elements.Get(new(context, parameter));
		return new(context, disposable, elements);
	}
}