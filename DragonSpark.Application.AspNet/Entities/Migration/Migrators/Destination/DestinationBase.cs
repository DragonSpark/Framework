using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

/* TODO: Enable Parallelism
class DestinationBase<TFrom, TTo> : IDestination<TFrom, TTo> where TTo : class where TFrom : class
{
	readonly IRunner<TFrom, TTo> _runner;

	protected DestinationBase(IInstance<TFrom, TTo> instance, IMap map)
		: this(new Element<TFrom, TTo>(instance, map)) {}

	protected DestinationBase(IElement<TFrom, TTo> element) : this(new Writer<TFrom, TTo>(element)) {}

	protected DestinationBase(IWriter<TFrom, TTo> writer) : this(new Runner<TFrom, TTo>(writer)) {}

	protected DestinationBase(IRunner<TFrom, TTo> runner) => _runner = runner;

	public IAsyncEnumerable<TTo> Get(Stop<DestinationInput<TFrom>> parameter)
	{
		var (_, stop) = parameter;

		var (work, reader) = _runner.Get(parameter);

		_ = Task.Run(work.Self, stop);

		return reader.ReadAllAsync(stop);
	}
}
*/
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
sealed class Parallelism : Instance<byte>
{
	public static Parallelism Default { get; } = new();

	Parallelism() : base(32) {}
}

sealed class DestinationChannel<T> : IResult<Channel<T>>
{
	public static DestinationChannel<T> Default { get; } = new();

	DestinationChannel() : this(Parallelism.Default) {}

	readonly byte _parallelism;

	public DestinationChannel(byte parallelism) => _parallelism = parallelism;

	public Channel<T> Get() => Channel.CreateBounded<T>(new BoundedChannelOptions(_parallelism)
	{
		SingleReader = true,
		FullMode     = BoundedChannelFullMode.Wait
	});
}

public interface IRunner<TFrom, TTo> : ISelect<Stop<DestinationInput<TFrom>>, RunnerResult<TTo>>;

sealed class Runner<TFrom, TTo> : IRunner<TFrom, TTo>
{
	readonly IResult<Channel<TTo>> _channel;
	readonly IWriter<TFrom, TTo>   _writer;

	public Runner(IWriter<TFrom, TTo> writer) : this(DestinationChannel<TTo>.Default, writer) {}

	public Runner(IResult<Channel<TTo>> channel, IWriter<TFrom, TTo> writer)
	{
		_channel = channel;
		_writer  = writer;
	}

	public RunnerResult<TTo> Get(Stop<DestinationInput<TFrom>> parameter)
	{
		var ((_, source, destination, from, _), stop) = parameter;

		var channel = _channel.Get();
		var work    = _writer.Get(new(new(source, destination, from, channel.Writer), stop));
		return new(work, channel.Reader);
	}
}

public readonly record struct RunnerResult<T>(Task Work, ChannelReader<T> Reader);

public readonly record struct WriterInput<TFrom, TTo>(
	DbContext Source,
	DbContext Destination,
	Array<TFrom> Items,
	ChannelWriter<TTo> Writer);

public interface IWriter<TFrom, TTo> : IAllocated<WriterInput<TFrom, TTo>>;

sealed class Writer<TFrom, TTo> : IWriter<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly IElement<TFrom, TTo> _element;
	readonly byte                 _parallelism;

	public Writer(IElement<TFrom, TTo> element) : this(element, Parallelism.Default) {}

	public Writer(IElement<TFrom, TTo> element, byte parallelism)
	{
		_element     = element;
		_parallelism = parallelism;
	}

	public async Task Get(Stop<WriterInput<TFrom, TTo>> parameter)
	{
		var ((source, destination, from, writer), stop) = parameter;
		try
		{
			var options = new ParallelOptions { MaxDegreeOfParallelism = _parallelism, CancellationToken = stop };
			await Parallel.ForEachAsync(from.Open(), options,
			                            async (x, ct) =>
			                            {
				                            var item = await _element.Off(new(new(source, destination, from, x), ct));
				                            await writer.WriteAsync(item, ct).Off();
			                            })
			              .Off();

			writer.Complete();
		}
		catch (Exception ex)
		{
			writer.Complete(ex);
		}
	}
}

public interface IElement<TFrom, TTo> : IAllocated<MappingInput<TFrom>, TTo> where TFrom : class where TTo : class;

sealed class Element<TFrom, TTo> : IElement<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly IInstance<TFrom, TTo> _instance;
	readonly IMap                  _map;

	public Element(IInstance<TFrom, TTo> instance, IMap map)
	{
		_instance = instance;
		_map      = map;
	}

	public async Task<TTo> Get(Stop<MappingInput<TFrom>> parameter)
	{
		var ((source, destination, _, from), stop) = parameter;
		var result = await _instance.Off(parameter);
		await _map.Off(new(new(source.Entry(from), destination.Entry(result)), stop));
		return result;
	}
}