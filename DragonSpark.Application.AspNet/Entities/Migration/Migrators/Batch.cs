using DragonSpark.Compose;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Logging;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

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