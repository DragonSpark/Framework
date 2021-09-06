﻿using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class InvocationComposer<TIn, TContext, T> : IResult<IInvoke<TIn, T>> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IQuery<TIn, T>      _query;

		public InvocationComposer(IContexts<TContext> contexts, IQuery<TIn, T> query)
		{
			_contexts = contexts;
			_query    = query;
		}

		public InvocationComposer<TIn, TContext, TTo> Select<TTo>(
			Func<QueryComposer<TIn, T>, QueryComposer<TIn, TTo>> select)
			=> new(_contexts, select(new QueryComposer<TIn, T>(_query)).Get());

		public QueryInvocationComposer<TIn, T> To => new(Get());

		public EditInvocationComposer<TIn, T> Edit => new(Get());

		public ISelect<TIn, IQueryable<T>> Compile()
			=> new Compiled<TContext, TIn, T>(_contexts, _query.Get().Expand().Compile());

		public IInvoke<TIn, T> Get() => new Invoke<TContext,TIn,T>(_contexts, _query.Get());
	}
}