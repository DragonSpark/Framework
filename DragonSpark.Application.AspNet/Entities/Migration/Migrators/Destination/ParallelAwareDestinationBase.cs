using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

class ParallelAwareDestinationBase<TFrom, TTo> : IDestination<TFrom> where TTo : class where TFrom : class
{
	readonly IRunner<TFrom> _runner;

	protected ParallelAwareDestinationBase(IEntry<TFrom, TTo> entry, IMap map)
		: this(new Element<TFrom, TTo>(entry, map)) {}

	protected ParallelAwareDestinationBase(IElement<TFrom, TTo> element) : this(new Writer<TFrom, TTo>(element)) {}

	protected ParallelAwareDestinationBase(IWriter<TFrom> writer) : this(new Runner<TFrom>(writer)) {}

	protected ParallelAwareDestinationBase(IRunner<TFrom> runner) => _runner = runner;

	public IAsyncEnumerable<DbContext> Get(Stop<DestinationInput<TFrom>> parameter)
	{
		var (_, stop) = parameter;

		var (work, reader) = _runner.Get(parameter);

		_ = Task.Run(work.Self, stop);

		return reader.ReadAllAsync(stop);
	}
}

// TODO
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
		var ((_, origin, workspaces, from, _), stop) = parameter;
		var original = workspaces.Get();
		var modified = original with { Source = origin };
		var (source, destination) = modified;
		foreach (var x in from.Open())
		{
			var to = await _entry.Off(new(new(workspaces, modified, origin.Entry(x)), stop));
			await _map.Off(new(new(source.Entry(x), destination.Entry(to.Instance)), stop));
		}

		await original.Source.DisposeAsync().Off();
		yield return destination;
	}
}

public sealed class MaxParallelism : Instance<byte>
{
	public static MaxParallelism Default { get; } = new();

	MaxParallelism() : base(32) {}
}

sealed class DestinationChannel<T> : IResult<Channel<T>>
{
	public static DestinationChannel<T> Default { get; } = new();

	DestinationChannel() : this(MaxParallelism.Default) {}

	readonly byte _parallelism;

	public DestinationChannel(byte parallelism) => _parallelism = parallelism;

	public Channel<T> Get() => Channel.CreateBounded<T>(new BoundedChannelOptions(_parallelism)
	{
		SingleReader = true,
		FullMode     = BoundedChannelFullMode.Wait
	});
}

public interface IRunner<T> : ISelect<Stop<DestinationInput<T>>, RunnerResult>;

sealed class Runner<T> : IRunner<T>
{
	readonly IResult<Channel<DbContext>> _channel;
	readonly IWriter<T>                  _writer;

	public Runner(IWriter<T> writer) : this(DestinationChannel<DbContext>.Default, writer) {}

	public Runner(IResult<Channel<DbContext>> channel, IWriter<T> writer)
	{
		_channel = channel;
		_writer  = writer;
	}

	public RunnerResult Get(Stop<DestinationInput<T>> parameter)
	{
		var ((_, source, workspaces, from, _), stop) = parameter;

		var channel = _channel.Get();
		var work    = _writer.Get(new(new(source, workspaces, from, channel.Writer), stop));
		return new(work, channel.Reader);
	}
}

public readonly record struct RunnerResult(Task Work, ChannelReader<DbContext> Reader);

public readonly record struct WriterInput<T>(
	DbContext Source, // TODO
	IResult<Workspace> Workspaces,
	Array<T> Items,
	ChannelWriter<DbContext> Writer);

public interface IWriter<TFrom> : IAllocated<WriterInput<TFrom>>;

sealed class Writer<TFrom, TTo> : IWriter<TFrom> where TFrom : class where TTo : class
{
	readonly IElement<TFrom, TTo> _element;
	readonly byte                 _parallelism;

	public Writer(IElement<TFrom, TTo> element) : this(element, MaxParallelism.Default) {}

	public Writer(IElement<TFrom, TTo> element, byte parallelism)
	{
		_element     = element;
		_parallelism = parallelism;
	}

	public async Task Get(Stop<WriterInput<TFrom>> parameter)
	{
		var ((origin, destination, from, writer), stop) = parameter;
		try
		{
			var options = new ParallelOptions { MaxDegreeOfParallelism = _parallelism, CancellationToken = stop };
			await Parallel.ForEachAsync(from.Open(), options,
			                            new Each<TFrom, TTo>(new(origin, destination, from, writer), _element).Get)
			              .Off();

			writer.Complete();
		}
		catch (Exception ex)
		{
			writer.Complete(ex);
		}
	}
}

public readonly record struct EachInput<T>(
	DbContext Origin,
	IResult<Workspace> Workspace,
	Array<T> Page,
	ChannelWriter<DbContext> Writer);

sealed class Each<TFrom, TTo> : IStopAware<TFrom> where TFrom : class where TTo : class
{
	readonly EachInput<TFrom>     _paged;
	readonly IElement<TFrom, TTo> _element;
	readonly IMutable<DbContext?> _logical;

	public Each(EachInput<TFrom> paged, IElement<TFrom, TTo> element) : this(paged, element, LogicalContext.Default) {}

	public Each(EachInput<TFrom> paged, IElement<TFrom, TTo> element, IMutable<DbContext?> logical)
	{
		_paged   = paged;
		_element = element;
		_logical = logical;
	}

	public async ValueTask Get(Stop<TFrom> parameter)
	{
		var (subject, stop)                    = parameter;
		var (origin, workspaces, page, writer) = _paged;
		var workspace = workspaces.Get();
		var (source, destination) = workspace;
		using var _ = _logical.Assigned(destination);
		try
		{
			await _element.Off(new(new(workspaces, workspace, page, origin.Entry(subject)), stop));
			await writer.WriteAsync(destination, stop).Off();
			await source.DisposeAsync().Off();
		}
		catch
		{
			await workspace.DisposeAsync().Off();
			throw;
		}
	}
}

public interface IElement<TFrom, TTo> : IAllocated<MappingInput<TFrom>, TTo> where TFrom : class where TTo : class;

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
		var (to, values)                                            = await _entry.Off(parameter);
		var from  = source.Applied(current);
		var entry = destination.Entry(to);
		var next = entry.State == EntityState.Detached
			           ? values is not null
				             ? destination.Attach(entry.Assigned(values).Entity)
				             : destination.Add(entry.Assigned(current.CurrentValues).Entity)
			           : entry;
		await _map.Off(new(new(from, next), stop));
		return to;
	}
}