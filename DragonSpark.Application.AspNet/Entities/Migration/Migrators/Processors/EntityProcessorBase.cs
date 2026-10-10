using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

class EntityProcessorBase<TFrom, TTo> : IEntityProcessor<TFrom> where TFrom : class where TTo : class
{
	readonly ISource<TFrom>      _source;
	readonly IDestination<TFrom> _destination;
	readonly ISave               _save;

	protected EntityProcessorBase(ISource<TFrom> source, IDestination<TFrom> destination, ISave save)
	{
		_source      = source;
		_destination = destination;
		_save        = save;
	}

	public async ValueTask<uint> Get(Stop<SourceInput<TFrom>> parameter)
	{
		var ((logger, workspace, _, start, size, total), stop) = parameter;
		var watch  = Stopwatch.StartNew();
		var page   = await _source.Get(parameter).Skip(start.Degrade()).Take(size.Degrade()).ToArrayAsync(stop).Off();
		var result = page.Length.Grade();

		logger.LogInformation("{From} -> {To}: Processing {Start}/{Count} Items of {Total} ...",
		                      A.Type<TFrom>(), A.Type<TTo>(), start, result, total);

		await _destination.Off(new(new(logger, workspace, page, total), stop));

		var save = await _save.Off(new(new(logger, size, workspace.Destination, total), stop));

		logger.LogInformation("{From} -> {To}: Batch of {Count} ({Total} total/saved) processed in {Elapsed:mm\\:ss\\.fff} ({Rate:F1} entities/sec)",
		                      A.Type<TFrom>(), A.Type<TTo>(), result, save, watch.Elapsed,
		                      page.Length / watch.Elapsed.TotalSeconds);
		return result;
	}

	public uint? Get() => _save.Get();
}