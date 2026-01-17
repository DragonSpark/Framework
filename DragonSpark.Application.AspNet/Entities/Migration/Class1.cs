using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Invocation.Expressions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class DefaultBatchSize : Instance<ushort>
{
	public static DefaultBatchSize Default { get; } = new();

	DefaultBatchSize() : base(5_000) {}
}

public readonly record struct BatchesInput(ILogger Logger, ushort BatchSize)
{
	public BatchesInput(ILogger logger) : this(logger, DefaultBatchSize.Default) {}
}

public readonly record struct MapInput(EntityEntry From, EntityEntry To)
{
	public static MapInput New<T>(EntityEntry from, DbContext to) where T : class => new(from, to.Entry(A.New<T>()));
}

public interface IMap : ICommand<MapInput>;

// TODO

public readonly record struct MappingInput<T>(DbContext Source, DbContext Destination, T From);

public interface IMapping<TFrom, out TTo> : ISelect<MappingInput<TFrom>, TTo>;

public sealed class Mapping<TFrom, TTo> : IMapping<TFrom, TTo> where TFrom : class where TTo : class
{
	public static Mapping<TFrom, TTo> Default { get; } = new();

	Mapping() : this(Map.Default) {}

	readonly Func<TTo> _new;
	readonly IMap      _map;

	public Mapping(IMap map) : this(A.New<TTo>, map) {}

	public Mapping(Func<TTo> @new, IMap map)
	{
		_new = @new;
		_map = map;
	}

	public TTo Get(MappingInput<TFrom> parameter)
	{
		var (source, destination, from) = parameter;
		var result = _new();
		_map.Execute(new(source.Entry(from), destination.Entry(result)));
		return result;
	}
}

public record Batching(DbContext Source, DbContext Destination);

public sealed record Batching<T>(DbContext Source, DbContext Destination, IQueryable<T> Subject)
	: Batching(Source, Destination);

public class Migration<T> : Migration
{
	protected Migration(params IBatches[] batches) : this(DefaultLog<T>.Default.Get(), batches) {}

	protected Migration(ILogger logger, params IBatches[] batches) : base(logger, batches) {}
}

public class Migration : ICommand<ushort>, ICommand
{
	readonly ILogger         _logger;
	readonly Array<IBatches> _batches;

	protected Migration(ILogger logger, params IBatches[] batches)
	{
		_logger  = logger;
		_batches = batches;
	}

	public void Execute(None parameter)
	{
		Execute(DefaultBatchSize.Default);
	}

	public void Execute(ushort parameter)
	{
		var input = new BatchesInput(_logger, parameter);
		foreach (var batch in _batches)
		{
			batch.Execute(input);
		}
	}
}

public interface IBatches : ICommand<BatchesInput>, IResult<Batching>;

public class Batches<TFrom, TTo> : Instance<Batching>, IBatches where TFrom : class where TTo : class
{
	readonly Batching<TFrom> _batching;
	readonly IBatch<TFrom>   _batch;

	protected Batches(DbContext Source, DbContext Destination, IQueryable<TFrom> Subject)
		: this(new(Source, Destination, Subject)) {}

	protected Batches(Batching<TFrom> batching) : this(batching, Batch<TFrom, TTo>.Default) {}

	protected Batches(Batching<TFrom> batching, IBatch<TFrom> batch) : base(batching)
	{
		_batching = batching;
		_batch    = batch;
	}

	public void Execute(BatchesInput parameter)
	{
		var (logger, size)                 = parameter;
		var (source, destination, subject) = _batching;
		var total = subject.Count().Grade();
		for (var offset = 0; offset < total; offset += size)
		{
			_batch.Execute(new(logger, source, destination, subject, new(offset, size), total));
		}
	}
}

public sealed record BatchInput<T>(
	ILogger Logger,
	DbContext Source,
	DbContext Destination,
	IQueryable<T> From,
	Partition Partition,
	uint Total);

public interface IBatch<T> : ICommand<BatchInput<T>>;

public sealed class Batch<T, TTo> : IBatch<T> where T : class where TTo : class
{
	public static Batch<T, TTo> Default { get; } = new();

	Batch() : this(Mapping<T, TTo>.Default) {}

	readonly IMapping<T, TTo> _map;

	public Batch(IMapping<T, TTo> map) => _map = map;

	public void Execute(BatchInput<T> parameter)
	{
		var (logger, source, destination, from, (skip, top), total) = parameter;
		var watch  = Stopwatch.StartNew();
		var offset = skip.Value();
		using var batch = from.AsValueEnumerable()
		                      .Skip(offset)
		                      .Take(top.Value())
		                      .Select(x => _map.Get(new(source, destination, x)))
		                      .ToArray(ArrayPool<TTo>.Shared);

		var configuration = new BulkConfig { BatchSize = batch.Length, CalculateStats = true };
		destination.BulkInsertOrUpdate(batch, configuration);

		var info  = configuration.StatsInfo.Verify();
		var count = info.StatsNumberInserted + info.StatsNumberUpdated;
		logger.LogInformation("{From} -> {To}: Batch of {Count} processed in {Elapsed:mm\\:ss\\.fff} ({Rate:F1} entities/sec)",
		                      A.Type<T>(), A.Type<TTo>(), count, watch.Elapsed,
		                      count / watch.Elapsed.TotalSeconds);

		logger.LogDebug("Progress: {Processed}/{Total} ({Percent:F1}%)",
		                offset + count, total, (offset + count) / (double)total * 100);
	}
}

public static class Extensions
{
	public static IBatches Flatten<TFrom, TKey, TTo>(this Batches<TFrom, TTo> @this, Expression<Func<TFrom, TKey>> key)
		where TTo : class where TFrom : class
		=> new FlattenAwareBatches<TFrom, TKey, TTo>(@this, key);
}

sealed class FlattenAwareBatches<TFrom, TKey, TTo> : IBatches where TTo : class where TFrom : class
{
	readonly IBatches                      _previous;
	readonly Expression<Func<TFrom, TKey>> _key;
	readonly string                        _name;

	public FlattenAwareBatches(IBatches previous, Expression<Func<TFrom, TKey>> key)
		: this(previous, key, key.GetMemberInfo().Name) {}

	public FlattenAwareBatches(IBatches previous, Expression<Func<TFrom, TKey>> key, string name)
	{
		_previous = previous;
		_key      = key;
		_name     = name;
	}

	public void Execute(BatchesInput parameter)
	{
		var (logger, _)           = parameter;
		var (source, destination) = _previous.Get();

		// Query source PKs (assume Id property—adjust if key different)
		var existing = destination.Set<TTo>().Select(x => EF.Property<TKey>(x, _name));
		var exists   = source.Set<TFrom>().Select(_key).ToHashSet().IsSubsetOf(existing);
		var to       = destination.Set<TTo>();
		if (exists)
		{
			logger.LogInformation("Flatten {Set}: All source keys already present in destination (idempotent, no missing data)",
			                      to.GetType());
		}
		else
		{
			var cleared = to.ExecuteDelete();
			logger.LogInformation("Flatten {Set}: Cleared of {Count} entries", to.GetType(), cleared);
			_previous.Execute(parameter);
		}
	}

	public Batching Get() => _previous.Get();
}