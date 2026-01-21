using DragonSpark.Compose;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

class EntityProcessorBase<TFrom, TTo> : IEntityProcessor<TFrom> where TFrom : class where TTo : class
{
	readonly IProcessChanges<TFrom> _changes;

	protected EntityProcessorBase(IEntities<TFrom, TTo> entities, ISave<TTo> save)
		: this(new ProcessChanges<TFrom, TTo>(entities, save)) {}

	protected EntityProcessorBase(IProcessChanges<TFrom> changes) => _changes = changes;

	public void Execute(ProcessChangesInput<TFrom> parameter)
	{
		var (logger, _, source, destination, _, total) = parameter;
		if (total > 0)
		{
			var watch = Stopwatch.StartNew();
			var count = _changes.Get(parameter);

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