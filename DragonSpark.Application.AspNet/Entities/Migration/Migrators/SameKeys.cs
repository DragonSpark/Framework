using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class SameKeys<TFrom, TTo> : ICondition<MigrationInput> where TFrom : class where TTo : class
{
	public static SameKeys<TFrom,TTo> Default { get; } = new();

	SameKeys() : this(ComposeKnownKeys<TFrom>.Default, ComposeKnownKeys<TTo>.Default) {}
	
	readonly ISelect<DbContext, ImmutableHashSet<object>> _source;
	readonly ISelect<DbContext, ImmutableHashSet<object>> _destination;

	public SameKeys(ISelect<DbContext, ImmutableHashSet<object>> source, ISelect<DbContext, ImmutableHashSet<object>> destination)
	{
		_source           = source;
		_destination = destination;
	}

	public bool Get(MigrationInput parameter)
	{
		var (source, destination) = parameter;
		return _source.Get(source).IsSubsetOf(_destination.Get(destination));
	}
}