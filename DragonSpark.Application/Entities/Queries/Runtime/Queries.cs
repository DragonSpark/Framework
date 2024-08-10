﻿using DragonSpark.Model;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime;

sealed class Queries<TIn, TOut> : IQueries<TOut>
{
	readonly IScopes                                _scopes;
	readonly TIn                                    _parameter;
	readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

	public Queries(IScopes scopes, TIn parameter, Func<DbContext, TIn, IQueryable<TOut>> compiled)
	{
		_scopes    = scopes;
		_parameter = parameter;
		_compiled  = compiled;
	}

	public Query<TOut> Get()
	{
		var (context, disposable) = _scopes.Get();
		var query   = _compiled(context, _parameter);
		return new(query, disposable);
	}
}

// TODO

public sealed class EmptyQueries<T> : IQueries<T>
{
	public static EmptyQueries<T> Default { get; } = new();

	EmptyQueries() {}

	public Query<T> Get() => new(Empty.Queryable<T>(), EmptyDisposable.Default);
}