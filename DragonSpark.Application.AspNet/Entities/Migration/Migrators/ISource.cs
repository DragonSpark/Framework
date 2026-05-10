using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface ISource<T> : ISelect<Stop<SourceInput<T>>, IQueryable<T>>;

public sealed class Source<T> : ISource<T>
{
	public static Source<T> Default { get; } = new();

	Source() {}

	public IQueryable<T> Get(Stop<SourceInput<T>> parameter) => parameter.Subject.From;
}

// TODO V2

public interface IDestination<TFrom, TTo> : ISelect<Stop<DestinationInput<TFrom>>, IAsyncEnumerable<TTo>>;

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

public sealed record DestinationInput<T>(
	ILogger Logger,
	DbContext Source,
	DbContext Destination,
	Array<T> From,
	uint Total);