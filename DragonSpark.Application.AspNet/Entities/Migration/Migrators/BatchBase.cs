using DragonSpark.Compose;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

class BatchBase<TFrom, TTo> : IBatch<TFrom> where TFrom : class where TTo : class
{
	readonly IProcessChanges<TFrom> _process;

	protected BatchBase(IComposeBatch<TFrom, TTo> compose, ISaveBatch<TTo> save)
		: this(new ProcessChanges<TFrom, TTo>(compose, save)) {}

	protected BatchBase(IProcessChanges<TFrom> process) => _process = process;

	public void Execute(BatchInput<TFrom> parameter)
	{
		var (logger, _, _, _, (skip, _), total) = parameter;
		var watch  = Stopwatch.StartNew();
		var offset = skip.Value();
		var count  = _process.Get(parameter);

		logger.LogInformation("{From} -> {To}: Batch of {Count} processed in {Elapsed:mm\\:ss\\.fff} ({Rate:F1} entities/sec)",
		                      A.Type<TFrom>(), A.Type<TTo>(), count, watch.Elapsed,
		                      count / watch.Elapsed.TotalSeconds);

		logger.LogDebug("Progress: {Processed}/{Total} ({Percent:F1}%)",
		                offset + count, total, (offset + count) / (double)total * 100);
	}
}