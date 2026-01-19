using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
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
		
		_copy.Execute(parameter);

		foreach (var navigation in from.Metadata.GetNavigations().Where(x => x.TargetEntityType.IsOwned()))
		{
			_owned.Execute(new(from.Context.Entry(from.Entity).Navigation(navigation.Name),
			                   to.Context.Entry(to.Entity).Navigation(navigation.Name)));
		}
		to.Context.Attach(to.Entity);
		to.State = EntityState.Modified;
	}
}