using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ComposeSameKeys<TFrom, TTo> : ICondition where TFrom : class where TTo : class
{
	readonly Func<ImmutableHashSet<object>> _source;
	readonly Func<ImmutableHashSet<object>> _destination;

	public ComposeSameKeys(DbContext source, DbContext destination)
		: this(KnownKeys<TFrom>.Default.Then().Bind(source), KnownKeys<TTo>.Default.Then().Bind(destination)) {}

	public ComposeSameKeys(Func<ImmutableHashSet<object>> source, Func<ImmutableHashSet<object>> destination)
	{
		_source      = source;
		_destination = destination;
	}

	public bool Get(None parameter) => _source().IsSubsetOf(_destination());
}