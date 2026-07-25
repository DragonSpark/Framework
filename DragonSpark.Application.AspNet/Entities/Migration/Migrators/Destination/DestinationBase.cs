using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public class DestinationBase<TFrom, TTo> : IDestination<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly IInstance<TFrom, TTo> _instance;
	readonly IMap                  _map;

	public DestinationBase(IInstance<TFrom, TTo> instance, IMap map)
	{
		_instance = instance;
		_map      = map;
	}

	public async IAsyncEnumerable<TTo> Get(Stop<DestinationInput<TFrom>> parameter)
	{
		var ((_, source, destination, from, _), stop) = parameter;
		foreach (var x in from.Open())
		{
			var to = await _instance.Off(new(new(source, destination, from, x), stop));
			await _map.Off(new(new(source.Entry(x), destination.Entry(to)), stop));
			yield return to;
		}
	}
}