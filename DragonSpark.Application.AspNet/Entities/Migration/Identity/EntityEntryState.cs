using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

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