using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public class DestinationBase<TFrom, TTo> : IDestination<TFrom> where TFrom : class where TTo : class
{
	readonly IEntry<TFrom, TTo> _entry;
	readonly IMap               _map;

	protected DestinationBase(IEntry<TFrom, TTo> entry, IMap map)
	{
		_entry = entry;
		_map   = map;
	}

	public async IAsyncEnumerable<DbContext> Get(Stop<DestinationInput<TFrom>> parameter)
	{
		var ((_, entities, from, _), stop) = parameter;
		var original = entities.Get();
		var modified = original with { Source = entities.Origin };
		var (source, destination) = modified;
		foreach (var x in from.Open())
		{
			var to = await _entry.Off(new(new(entities, modified, entities.Origin.Entry(x)), stop));
			await _map.Off(new(new(source.Entry(x), destination.Entry(to.Instance)), stop));
		}

		await original.Source.DisposeAsync().Off();
		yield return destination;
	}
}