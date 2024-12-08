using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Model.Operations.Selection;
using System;

namespace DragonSpark.Application.AspNet.Entities;

public class Locate<TKey, T> : Coalesce<TKey, T> where T : class
{
	protected Locate(IScopes scopes, Func<T, TKey> key, IQuery<TKey, T> query)
		: this(scopes, key, scopes.Then().Use(query).To.Single()) {}

	protected Locate(IScopes scopes, Func<T, TKey> key, ISelecting<TKey, T> second)
		: base(new Local<TKey, T>(scopes, key), second) {}
}