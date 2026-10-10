using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

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

	public async ValueTask Get(Stop<PageInput<TFrom>> parameter)
	{
		var ((_, workspace, page, _), stop) = parameter;
		var (source, destination)           = workspace;
		foreach (var x in page.Open())
		{
			var from = source.Entry(x);
			var to   = await _entry.Off(new(new(workspace, page, from), stop));
			await _map.Off(new(new(from, destination.Entry(to.Instance)), stop));
		}
	}
}