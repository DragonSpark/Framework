using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class DefaultBatchSize : Instance<ushort>
{
	public static DefaultBatchSize Default { get; } = new();

	DefaultBatchSize() : base(5_000) {}
}

public readonly record struct EntityMigratorInput(ILogger Logger, ushort BatchSize)
{
	public EntityMigratorInput(ILogger logger) : this(logger, DefaultBatchSize.Default) {}
}

public readonly record struct MapInput(EntityEntry From, EntityEntry To)
{
	public static MapInput New<T>(EntityEntry from, DbContext to) where T : class => new(from, to.Entry(A.New<T>()));
}

public interface IMap : ICommand<MapInput>;

// TODO

public readonly record struct MappingInput(DbContext Source, DbContext Destination, object From, Type To);

public interface IMapped : ISelect<MappingInput, object>;

public sealed class Mapped : IMapped
{
	public static Mapped Default { get; } = new();

	Mapped() : this(Map.Default) {}

	readonly Func<Type, object> _new;
	readonly IMap               _map;

	public Mapped(IMap map) : this(A.New, map) {}

	public Mapped(Func<Type, object> @new, IMap map)
	{
		_new = @new;
		_map = map;
	}

	public object Get(MappingInput parameter)
	{
		var (source, destination, from, to) = parameter;
		var result = _new(to);
		_map.Execute(new(source.Entry(from), destination.Entry(result)));
		return result;
	}
}

public record Batching(DbContext Source, DbContext Destination);

public sealed record Batching<T>(DbContext Source, DbContext Destination, IQueryable<T> Subject)
	: Batching(Source, Destination) where T : class
{
	public Batching(DbContext Source, DbContext Destination) : this(Source, Destination, Source.Set<T>()) {}
}

public class Migration<T> : Migration
{
	protected Migration(DbContext source, DbContext destination, IEntityMigrators processors)
		: this(processors.Get(new(source, destination))) {}

	protected Migration(params IEntityMigrator[] migrators) : this(DefaultLog<T>.Default.Get(), migrators) {}

	protected Migration(ILogger logger, params IEntityMigrator[] migrators) : base(logger, migrators) {}
}

public static class Extensions
{
	public static IMigration WithConstraintManagement(this IMigration @this, DbContext destination)
		=> new ConstraintAwareMigration(@this, destination.Database);
}

public sealed class ConstraintAwareMigration : IMigration
{
	readonly IMigration     _previous;
	readonly DatabaseFacade _facade;

	public ConstraintAwareMigration(IMigration previous, DatabaseFacade facade)
	{
		_previous = previous;
		_facade   = facade;
	}

	public void Execute(ushort parameter)
	{
		_facade.ExecuteSqlRaw("EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';");
		try
		{
			_previous.Execute(parameter);
		}
		finally
		{
			_facade.ExecuteSqlRaw("EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';");	
		}
	}
}

public interface IMigration : ICommand<ushort>;

public class Migration : IMigration, ICommand
{
	readonly ILogger                _logger;
	readonly Array<IEntityMigrator> _migrators;

	protected Migration(ILogger logger, params IEntityMigrator[] migrators)
	{
		_logger    = logger;
		_migrators = migrators;
	}

	public void Execute(None parameter)
	{
		Execute(DefaultBatchSize.Default);
	}

	public void Execute(ushort parameter)
	{
		var input = new EntityMigratorInput(_logger, parameter);
		foreach (var batch in _migrators)
		{
			batch.Execute(input);
		}
	}
}

public sealed record EntityTypeMapping(Type From, Type To);

public interface IEntityMigrator : ICommand<EntityMigratorInput>, IResult<EntityTypeMapping>;

public class EntityMigratorBase<TFrom, TTo> : Instance<EntityTypeMapping>, IEntityMigrator
	where TFrom : class where TTo : class
{
	readonly Batching<TFrom> _batching;
	readonly IBatch<TFrom>   _batch;

	protected EntityMigratorBase(Batching<TFrom> batching, IMap map) : this(batching, new Batch<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(Batching<TFrom> batching, IBatch<TFrom> batch) : base(new(typeof(TFrom), typeof(TTo)))
	{
		_batching = batching;
		_batch    = batch;
	}

	public void Execute(EntityMigratorInput parameter)
	{
		var (logger, size)                 = parameter;
		var (source, destination, subject) = _batching;
		var total = subject.Count().Grade();
		for (var offset = 0; offset < total; offset += size)
		{
			_batch.Execute(new(logger, source, destination, subject, new(offset, size), total));
		}

		source.ChangeTracker.Clear();
		destination.ChangeTracker.Clear();
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

public sealed class Batch<TFrom, TTo> : IBatch<TFrom> where TFrom : class where TTo : class
{
	readonly IMapped        _map;
	readonly Type           _to;
	readonly ArrayPool<TTo> _pool;

	public Batch(IMap map) : this(new Mapped(map), A.Type<TTo>(), ArrayPool<TTo>.Shared) {}

	public Batch(IMapped map, Type to, ArrayPool<TTo> pool)
	{
		_map  = map;
		_to   = to;
		_pool = pool;
	}

	public void Execute(BatchInput<TFrom> parameter)
	{
		var (logger, source, destination, from, (skip, top), total) = parameter;
		var watch  = Stopwatch.StartNew();
		var offset = skip.Value();
		using var batch = from.Skip(offset)
		                      .Take(top.Value())
		                      .Select(x => (TTo)_map.Get(new(source, destination, x, _to)))
		                      .AsValueEnumerable()
		                      .ToArray(_pool);

		var configuration = new BulkConfig { BatchSize = batch.Length, CalculateStats = true };
		destination.BulkInsertOrUpdate(batch, configuration);

		var info  = configuration.StatsInfo.Verify();
		var count = info.StatsNumberInserted + info.StatsNumberUpdated;
		logger.LogInformation("{From} -> {To}: Batch of {Count} processed in {Elapsed:mm\\:ss\\.fff} ({Rate:F1} entities/sec)",
		                      A.Type<TFrom>(), _to, count, watch.Elapsed,
		                      count / watch.Elapsed.TotalSeconds);

		logger.LogDebug("Progress: {Processed}/{Total} ({Percent:F1}%)",
		                offset + count, total, (offset + count) / (double)total * 100);
	}
}