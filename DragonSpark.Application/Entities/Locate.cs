using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Entities;

public class Locate<TKey, T> : Coalesce<T, T> where T : class
{
	protected Locate(IScopes scopes, Func<T, TKey> key, IQuery<T, T> query)
		: this(scopes, key, scopes.Then().Use(query).To.Single()) {}

	protected Locate(IScopes scopes, Func<T, TKey> key, ISelecting<T, T> second)
		: base(new Local<TKey, T>(scopes, key), second) {}
}