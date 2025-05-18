using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;
using System;

namespace DragonSpark.Application.AspNet.Entities;

public class Locate<TKey, T> : Coalesce<Stop<TKey>, T> where T : class
{
	protected Locate(IScopes scopes, Func<T, TKey> key, IQuery<TKey, T> query)
		: this(scopes, key, scopes.Then().Use(query).To.Single()) {}

	protected Locate(IScopes scopes, Func<T, TKey> key, IStopAware<TKey, T> second)
		: base(new Local<TKey, T>(scopes, key).AsStop(), second) {}
}