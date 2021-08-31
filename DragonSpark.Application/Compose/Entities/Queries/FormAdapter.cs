using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Evaluation;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class FormAdapter<TIn, TContext, T> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IQuery<TIn, T>      _query;

		public FormAdapter(IContexts<TContext> contexts, IQuery<TIn, T> query)
		{
			_contexts = contexts;
			_query    = query;
		}

		public ISelecting<TIn, Array<T>> Array() => new EvaluateToArray<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, Lease<T>> Lease() => new EvaluateToLease<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, List<T>> List() => new EvaluateToList<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, Dictionary<TKey, T>> Dictionary<TKey>(Func<T, TKey> key) where TKey : notnull
			=> new EvaluateToMap<TIn, TContext, T, TKey>(_contexts, _query, key);

		public ISelecting<TIn, Dictionary<TKey, TValue>> Dictionary<TKey, TValue>(
			Func<T, TKey> key, Func<T, TValue> value)
			where TKey : notnull
			=> new EvaluateToMappedSelection<TIn, TContext, T, TKey, TValue>(_contexts, _query,
			                                                                 new ToDictionary<T, TKey,
				                                                                 TValue>(key, value));

		public ISelecting<TIn, T> Single() => new EvaluateToSingle<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, T?> SingleOrDefault()
			=> new EvaluateToSingleOrDefault<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, T> First() => new EvaluateToFirst<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, T?> FirstOrDefault()
			=> new EvaluateToFirstOrDefault<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, bool> Any() => new EvaluateToAny<TIn, TContext, T>(_contexts, _query);
	}
}