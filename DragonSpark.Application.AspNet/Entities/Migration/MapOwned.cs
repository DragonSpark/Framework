using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

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