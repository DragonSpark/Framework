using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;

sealed class LocateAwareEntry<TFrom, TTo> : IEntry<TFrom, TTo> where TTo : class where TFrom : class
{
	public static LocateAwareEntry<TFrom, TTo> Default { get; } = new();

	LocateAwareEntry() : this(EntityMaps<TFrom, TTo>.Default, Activate<TFrom, TTo>.Default, Keys.Default) {}

	readonly IEntityMaps<TFrom, TTo>      _maps;
	readonly IEntry<TFrom, TTo>           _previous;
	readonly ISelect<EntityEntry, object> _key;

	public LocateAwareEntry(IEntityMaps<TFrom, TTo> maps, IEntry<TFrom, TTo> previous,
	                        ISelect<EntityEntry, object> key)
	{
		_maps     = maps;
		_previous = previous;
		_key      = key;
	}

	public async ValueTask<Entry<TTo>> Get(Stop<MappingInput<TFrom>> parameter)
	{
		var ((workspaces, _, page, current), stop) = parameter;
		var map = await _maps.Get(workspaces).Off(new(page.Open(), stop));
		var key = _key.Get(current);
		var pop = map.TryPop(key, out var existing);
		return pop ? existing : await _previous.Off(parameter);
	}
}