using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model.Operations.Selection;
using System;

namespace DragonSpark.Application.Entities;

public class Locate<TKey, T> : Coalesce<TKey, T> where T : class
{
	protected Locate(IContexts contexts, Func<T, TKey> key, IQuery<TKey, T> query)
		: this(contexts, key, contexts.Then().Use(query).To.Single()) {}

	protected Locate(IContexts contexts, Func<T, TKey> key, ISelecting<TKey, T> second)
		: base(new Local<TKey, T>(contexts, key), second) {}
}