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

sealed class Pages<T> : IStopAware<MigrateInput, uint>
{
	readonly Func<DbContext, IQueryable<T>> _query;
	readonly IEntityProcessor<T>            _processor;
	readonly ushort                         _parallelism;

	public Pages(Func<DbContext, IQueryable<T>> query, IEntityProcessor<T> processor)
		: this(query, processor, MaximumParallelism.Default) {}

	public Pages(Func<DbContext, IQueryable<T>> query, IEntityProcessor<T> processor, ushort parallelism)
	{
		_query       = query;
		_processor   = processor;
		_parallelism = parallelism;
	}

	public async ValueTask<uint> Get(Stop<MigrateInput> parameter)
	{
		var ((logger, workspaces, (minimum, maximum, total)), stop) = parameter;

		var batch   = _processor.Get() ?? (total < minimum ? total : Math.Clamp(total / _parallelism, minimum, maximum));
		var results = new ConcurrentStack<uint>();
		var input   = new PageInput<T>(logger, workspaces, _query, batch, total, results);
		var page    = new Page<T>(_processor, input);
		var pages   = (int)Math.Ceiling((double)total / batch);
		switch (pages)
		{
			case 1:
				await page.Off(new(0, stop));
				break;
			default:
			{
				var options = new ParallelOptions { MaxDegreeOfParallelism = _parallelism, CancellationToken = stop };
				using var offsets = Enumerable.Range(0, pages)
				                              .Select(i => (uint)(i * batch))
				                              .AsValueEnumerable()
				                              .ToArray(ArrayPool<uint>.Shared);

				await Parallel.ForEachAsync(offsets, options, page.Get).Off();
				break;
			}
		}

		var result = (uint)results.Sum(x => x);
		return result;
	}
}