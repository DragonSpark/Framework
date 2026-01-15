using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

class Class2;

// TODO

sealed class MapEntries : IMap
{
	public static MapEntries Default { get; } = new();

	MapEntries() : this(MapOwned.Default) {}

	readonly ICommand<MapNavigationEntryInput> _owned;

	public MapEntries(ICommand<MapNavigationEntryInput> owned) => _owned = owned;

	public void Execute(MapInput parameter)
	{
		var (from, to) = parameter;
		to.Context.Attach(to.Entity);
		to.CurrentValues.SetValues(from.Entity);

		foreach (var navigation in from.Metadata.GetNavigations().Where(x => x.TargetEntityType.IsOwned()))
		{
			_owned.Execute(new(from.Context.Entry(from.Entity).Navigation(navigation.Name),
			                   to.Context.Entry(to.Entity).Navigation(navigation.Name)));
		}
	}
}

public readonly record struct MapNavigationEntryInput(NavigationEntry From, NavigationEntry To);

sealed class MapOwned : ICommand<MapNavigationEntryInput>
{
	/*
	static void Collection(NavigationEntry source, EntityEntry destination, INavigation navigation)
	{
		// Collection: clear + add mapped
		var to = destination.Collection(navigation.Name);
		if (to.CurrentValue is IList t)
		{
			t.Clear();
			if (source.CurrentValue is IEnumerable from)
			{
				foreach (var fromChild in from.Cast<object>())
				{
					/*A.New(to.Metadata.GetCollectionAccessor().CollectionType)
					var mappedChild = GetMappedChild(fromChild);
					if (mappedChild != null)
					{
						t.Add(mappedChild);
					}#1#
				}
			}
		}
	}
	*/

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