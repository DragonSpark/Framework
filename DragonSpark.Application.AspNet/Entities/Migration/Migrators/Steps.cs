using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Runtime.Invocation;
using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Concurrent;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Steps<T> : IStopAware<MigrateInput, uint>
{
	readonly Func<DbContext, IQueryable<T>> _query;
	readonly IEntityProcessor<T>            _processor;
	readonly ushort                         _parallelism;

	public Steps(Func<DbContext, IQueryable<T>> query, IEntityProcessor<T> processor)
		: this(query, processor, MaximumParallelism.Default) {}

	public Steps(Func<DbContext, IQueryable<T>> query, IEntityProcessor<T> processor, ushort parallelism)
	{
		_query       = query;
		_processor   = processor;
		_parallelism = parallelism;
	}

	public async ValueTask<uint> Get(Stop<MigrateInput> parameter)
	{
		var ((logger, workspaces, size, total), stop) = parameter;

		var results = new ConcurrentStack<uint>();
		var pages   = (int)Math.Ceiling((double)total / size);
		using var offsets = Enumerable.Range(0, pages)
		                              .Select(i => (uint)(i * size))
		                              .AsValueEnumerable()
		                              .ToArray(ArrayPool<uint>.Shared);
		var step    = new Step<T>(new(logger, workspaces, _query, size, total, _processor, results));
		var options = new ParallelOptions { MaxDegreeOfParallelism = _parallelism, CancellationToken = stop };
		await Parallel.ForEachAsync(offsets, options, step.Get).Off();

		var result = (uint)results.Sum(x => x);
		return result;
	}
}