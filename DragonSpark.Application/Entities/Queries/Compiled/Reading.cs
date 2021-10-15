using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled;

public readonly struct Reading<T> : IDisposable
{
	readonly IDisposable _disposable;

	public Reading(DbContext context, IDisposable disposable, IAsyncEnumerable<T> elements)
	{
		_disposable = disposable;
		Context     = context;
		Elements    = elements;
	}

	public DbContext Context { get; }

	public IAsyncEnumerable<T> Elements { get; }

	public void Deconstruct(out DbContext context, out IDisposable disposable, out IAsyncEnumerable<T> elements)
	{
		context    = Context;
		disposable = _disposable;
		elements   = Elements;
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}
}

public class Reading<TIn, T> : IReading<TIn, T>
{
	readonly IScopes           _scopes;
	readonly IElements<TIn, T> _elements;

	public Reading(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(scopes, new Elements<TIn, T>(expression)) {}

	public Reading(IScopes scopes, IElements<TIn, T> elements)
	{
		_scopes   = scopes;
		_elements = elements;
	}

	public async ValueTask<Reading<T>> Get(TIn parameter)
	{
		var (context, boundary) = _scopes.Get();
		var elements = _elements.Get(new(context, parameter));
		return new(context, await boundary.Get(), elements);
	}
}