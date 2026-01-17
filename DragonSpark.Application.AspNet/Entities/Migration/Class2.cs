using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class MapEntries : IMap
{
	public static MapEntries Default { get; } = new();

	MapEntries() : this(CopyValues.Default, MapOwned.Default) {}

	readonly ICommand<MapInput>                _copy;
	readonly ICommand<MapNavigationEntryInput> _owned;

	public MapEntries(ICommand<MapInput> copy, ICommand<MapNavigationEntryInput> owned)
	{
		_copy  = copy;
		_owned = owned;
	}

	public void Execute(MapInput parameter)
	{
		var (from, to) = parameter;
		to.Context.Attach(to.Entity);

		_copy.Execute(parameter);

		foreach (var navigation in from.Metadata.GetNavigations().Where(x => x.TargetEntityType.IsOwned()))
		{
			_owned.Execute(new(from.Context.Entry(from.Entity).Navigation(navigation.Name),
			                   to.Context.Entry(to.Entity).Navigation(navigation.Name)));
		}
	}
}

// TODO

sealed class CopyValues : ICommand<MapInput>
{
	public static CopyValues Default { get; } = new();

	CopyValues() {}

	public void Execute(MapInput parameter)
	{
		var (from, to) = parameter;

		var values = from.CurrentValues.Properties.ToDictionary(x => x.Name, x => from.CurrentValues[x]);
		to.CurrentValues.SetValues(values);
	}
}

public readonly record struct MapNavigationEntryInput(NavigationEntry From, NavigationEntry To);

sealed class MapOwned : ICommand<MapNavigationEntryInput>
{
	public static MapOwned Default { get; } = new();

	MapOwned() {}

	public void Execute(MapNavigationEntryInput parameter)
	{
		var (from, to) = parameter;

		if (from.CurrentValue is not null)
		{
			to.CurrentValue ??= A.New(to.Metadata.TargetEntityType.ClrType);

			var source      = from.EntityEntry.Context.Entry(from.CurrentValue);
			var destination = to.EntityEntry.Context.Entry(to.CurrentValue);
			destination.CurrentValues.SetValues(source.Entity);

			foreach (var nestedNav in source.Metadata.GetNavigations().Where(n => n.TargetEntityType.IsOwned()))
			{
				Execute(new(source.Navigation(nestedNav.Name), destination.Navigation(nestedNav.Name)));
			}
		}
	}
}