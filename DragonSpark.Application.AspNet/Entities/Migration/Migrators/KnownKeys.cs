using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Reflection.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class KnownKeys<T> : ReferenceValueStore<DbContext, ImmutableHashSet<object>>, IKnownKeys where T : class
{
	public static KnownKeys<T> Default { get; } = new();

	KnownKeys() : base(ComposeKnownKeys<T>.Default) {}
}

// TODO V2
public interface IKnownKeys : ISelect<DbContext, ImmutableHashSet<object>>;

sealed class KnownKeys : ReferenceValueStore<DbContext, ITypeKeys>, ISelect<EntityEntry, ImmutableHashSet<object>>
{
	public static KnownKeys Default { get; } = new();

	KnownKeys() : base(x => new TypeKeys(new ComposeTypeKeys(x).Then().Stores().New())) {}

	public ImmutableHashSet<object> Get(EntityEntry parameter)
		=> Get(parameter.Context).Get(parameter.Metadata.ClrType);
}

sealed class EntityEntryState : ISelect<EntityEntry, EntityState>
{
	public static EntityEntryState Default { get; } = new();

	EntityEntryState() : this(KnownKeys.Default, Keys.Default) {}

	readonly ISelect<EntityEntry, ImmutableHashSet<object>> _all;
	readonly ISelect<EntityEntry, object>                   _keys;

	public EntityEntryState(ISelect<EntityEntry, ImmutableHashSet<object>> all,
	                        ISelect<EntityEntry, object> keys)
	{
		_all  = all;
		_keys = keys;
	}

	public EntityState Get(EntityEntry parameter)
		=> _all.Get(parameter).Contains(_keys.Get(parameter)) ? EntityState.Modified : EntityState.Added;
}

sealed class Keys : ISelect<EntityEntry, object>
{
	public static Keys Default { get; } = new();

	Keys() {}

	public object Get(EntityEntry parameter)
	{
		var properties = parameter.Metadata.FindPrimaryKey().Verify().Properties;

		switch (properties.Count)
		{
			case 1:
				return parameter.Property(properties[0].Name).CurrentValue.Verify();
			default:
				return properties.Select(p => parameter.Property(p.Name).CurrentValue.Verify()).ToArray();
		}
	}
}

public interface ITypeKeys : ISelect<Type, ImmutableHashSet<object>>;

sealed class TypeKeys : Select<Type, ImmutableHashSet<object>>, ITypeKeys
{
	public TypeKeys(ISelect<Type, ImmutableHashSet<object>> select) : base(select) {}
}

sealed class ComposeTypeKeys : ITypeKeys
{
	readonly DbContext            _context;
	readonly IGeneric<IKnownKeys> _keys;

	public ComposeTypeKeys(DbContext context)
		: this(context, Start.A.Generic(typeof(KnownKeys<>)).Of.Type<IKnownKeys>()) {}

	public ComposeTypeKeys(DbContext context, IGeneric<IKnownKeys> keys)
	{
		_context = context;
		_keys    = keys;
	}

	public ImmutableHashSet<object> Get(Type parameter) => _keys.Get(new[] { parameter })().Get(_context);
}