using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration;

/*public sealed class MapOwned : ICommand<MapOwnedInput>
{
	public static MapOwned Default { get; } = new();

	MapOwned() : this(Names.Default, AssignValue.Default, DetermineValue.Default) {}

	readonly ISelect<IEntityType, ImmutableHashSet<string>> _names;
	readonly IAssignValue                                   _assign;
	readonly ISelect<DetermineValueInput, object?>          _value;

	public MapOwned(ISelect<IEntityType, ImmutableHashSet<string>> names, IAssignValue assign,
	                ISelect<DetermineValueInput, object?> value)
	{
		_names  = names;
		_assign = assign;
		_value  = value;
	}

	public void Execute(MapOwnedInput parameter)
	{
		var (sourceValue, destinationNav, sourceNav) = parameter;

		if (sourceValue is not null)
		{
			destinationNav.CurrentValue ??= A.New(destinationNav.Metadata.TargetEntityType.ClrType);

			var destinationEntry = (destinationNav as ReferenceEntry)?.TargetEntry
			                       ?? destinationNav.EntityEntry.Context.Entry(destinationNav.CurrentValue!);

			Copy(sourceValue, sourceNav.TargetEntityType, destinationEntry);

			foreach (var nestedNav in sourceNav.TargetEntityType.GetNavigations()
			                                   .Where(n => n.TargetEntityType.IsOwned()))
			{
				var nestedSourceValue = nestedNav.GetGetter().GetClrValue(sourceValue);
				var nestedDestNav     = destinationEntry.Navigation(nestedNav.Name);

				Execute(new(nestedSourceValue, nestedDestNav, nestedNav));
			}
		}
	}

	void Copy(object sourceInstance, IEntityType sourceMetadata, EntityEntry destination)
	{
		var names = _names.Get(destination.Metadata);

		foreach (var property in sourceMetadata.GetProperties().Where(p => !p.IsPrimaryKey()))
		{
			var name = property.Name;

			if (names.Contains(name))
			{
				// 1. Thread-safe EF property getter
				var rawValue = property.GetGetter().GetClrValue(sourceInstance);

				// 2. Allow null/default values to pass through to DetermineValue input
				var determined = rawValue is not null ? _value.Get(new(name, rawValue, destination)) : null;

				// 3. Assign if DetermineValue produced a value (or rawValue was non-null)
				if (determined is not null)
				{
					_assign.Execute(new(determined, destination.Property(name)));
				}
				else if (rawValue is not null)
				{
					_assign.Execute(new(rawValue, destination.Property(name)));
				}
			}
		}
	}
}*/// TODO

sealed class MapOwned : ICommand<MapNavigationEntryInput>
{
	public static MapOwned Default { get; } = new();

	MapOwned() : this(CopyValues.Default) {}

	readonly ICommand<MapInput> _copy;

	public MapOwned(ICommand<MapInput> copy) => _copy = copy;

	public void Execute(MapNavigationEntryInput parameter)
	{
		var (from, to) = parameter;

		if (from.CurrentValue is not null)
		{
			to.CurrentValue ??= A.New(to.Metadata.TargetEntityType.ClrType);

			var source      = from.EntityEntry.Context.Entry(from.CurrentValue);
			var destination = to.EntityEntry.Context.Entry(to.CurrentValue);
			switch (from.EntityEntry.State)
			{
				case EntityState.Detached:
					from.EntityEntry.Context.Attach(from.EntityEntry.Entity);
					break;
			}

			_copy.Execute(new(source, destination));

			foreach (var nestedNav in source.Metadata.GetNavigations().Where(n => n.TargetEntityType.IsOwned()))
			{
				Execute(new(source.Navigation(nestedNav.Name), destination.Navigation(nestedNav.Name)));
			}
		}
	}
}

// TODO

sealed class DetermineEntry : ISelect<NavigationEntry, EntityEntry>
{
	public static DetermineEntry Default { get; } = new();

	DetermineEntry() {}

	public EntityEntry Get(NavigationEntry parameter)
		=> parameter is ReferenceEntry { TargetEntry: not null } reference
			   ? reference.TargetEntry
			   : parameter.EntityEntry.Context.Entry(parameter.CurrentValue.Verify());
}