using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;

sealed class LocateAwareInstance<TFrom, TTo> : IInstance<TFrom, TTo> where TTo : class where TFrom : class
{
	public static LocateAwareInstance<TFrom, TTo> Default { get; } = new();

	LocateAwareInstance() : this(EntityMaps<TFrom, TTo>.Default, Activate<TFrom, TTo>.Default, Keys.Default) {}

	readonly IEntityMaps<TFrom, TTo>      _maps;
	readonly IInstance<TFrom, TTo>        _previous;
	readonly ISelect<EntityEntry, object> _key;

	public LocateAwareInstance(IEntityMaps<TFrom, TTo> maps, IInstance<TFrom, TTo> previous,
	                           ISelect<EntityEntry, object> key)
	{
		_maps     = maps;
		_previous = previous;
		_key      = key;
	}

	public async ValueTask<TTo> Get(Stop<MappingInput<TFrom>> parameter)
	{
		var ((source, destination, page, from), stop) = parameter;

		var map    = await _maps.Get(source).Get(destination).Off(new(page.Open(), stop));
		var key    = _key.Get(source.Entry(from));
		var exists = map.TryGet(key, out var existing);
		return exists ? existing : await _previous.Off(parameter);
	}
}