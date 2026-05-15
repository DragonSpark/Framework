using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

class EntityProcessorBase<TFrom, TTo> : IEntityProcessor<TFrom> where TFrom : class where TTo : class
{
	readonly ISource<TFrom>           _source;
	readonly IDestination<TFrom, TTo> _destination;
	readonly ISave<TTo>               _save;

	protected EntityProcessorBase(ISource<TFrom> source, IDestination<TFrom, TTo> destination, ISave<TTo> save)
	{
		_source      = source;
		_destination = destination;
		_save        = save;
	}

	public async ValueTask Get(Stop<SourceInput<TFrom>> parameter)
	{
		var ((logger, size, source, destination, _, total), stop) = parameter;
		if (total > 0)
		{
			logger.LogInformation("{From} -> {To}: Starting...", A.Type<TFrom>(), A.Type<TTo>());
			var watch = Stopwatch.StartNew();
			var count = 0u;

			await using var transaction = await destination.Database.BeginTransactionAsync(stop).Off();
			await foreach (var page in _source.Get(parameter).AsAsyncEnumerable().Chunk(size).WithCancellation(stop))
			{
				var to = await _destination.Get(new(new(logger, source, destination, page, total), stop))
				                           .ToArrayAsync()
				                           .Off();
				count += await _save.Off(new(new(logger, size, destination, to, total), stop));
				destination.ChangeTracker.Clear();
			}
			await transaction.CommitAsync().Off();

			logger.LogInformation("{From} -> {To}: Batch of {Count} processed in {Elapsed:mm\\:ss\\.fff} ({Rate:F1} entities/sec)",
			                      A.Type<TFrom>(), A.Type<TTo>(), count, watch.Elapsed,
			                      count / watch.Elapsed.TotalSeconds);

			source.ChangeTracker.Clear();
			destination.ChangeTracker.Clear();
		}
		else
		{
			logger.LogInformation("{From} -> {To}: No rows found in source", A.Type<TFrom>(), A.Type<TTo>());
		}
	}
}