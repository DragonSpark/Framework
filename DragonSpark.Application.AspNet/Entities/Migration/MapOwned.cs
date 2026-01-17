using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

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