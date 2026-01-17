using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class Map : IMap
{
	public static Map Default { get; } = new();

	Map() : this(CopyValues.Default, MapOwned.Default) {}

	readonly ICommand<MapInput>                _copy;
	readonly ICommand<MapNavigationEntryInput> _owned;

	public Map(ICommand<MapInput> copy, ICommand<MapNavigationEntryInput> owned)
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

		var compose = new DetermineValue(from.CurrentValues);
		var values  = from.CurrentValues.Properties.ToDictionary(x => x.Name, compose.Get);
		to.CurrentValues.SetValues(values);
	}
}

sealed class DetermineValue : ISelect<IProperty, object?>
{
	readonly PropertyValues _previous;

	public DetermineValue(PropertyValues previous) => _previous = previous;

	public object? Get(IProperty parameter)
		=> /*parameter.ClrType.IsEnum
			   ? Convert.ChangeType(_previous[parameter], parameter.ClrType.GetEnumUnderlyingType())
			   :*/ _previous[parameter];
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