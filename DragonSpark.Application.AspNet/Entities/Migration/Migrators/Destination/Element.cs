using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class Element<TFrom, TTo> : IElement<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly IEntry<TFrom, TTo> _entry;
	readonly IMap               _map;

	public Element(IEntry<TFrom, TTo> entry, IMap map)
	{
		_entry = entry;
		_map   = map;
	}

	public async Task<TTo> Get(Stop<MappingInput<TFrom>> parameter)
	{
		var ((_, (source, destination), _, current), stop) = parameter;
		var (to, values) = await _entry.Off(parameter);
		var entry = destination.Entry(to);
		var next = entry.State == EntityState.Detached
			           ? values is not null
				             ? destination.Attach(entry.Assigned(values).Entity)
				             : destination.Add(entry.Assigned(current.CurrentValues).Entity)
			           : entry;
		await _map.Off(new(new(source.Applied(current), next), stop));
		return to;
	}
}