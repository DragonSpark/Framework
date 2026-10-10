using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Migrate<TFrom, TTo> : IStopAware<EntityMigratorInput>
{
	readonly IStopAware<IWorkspaces, uint>  _total;
	readonly IStopAware<MigrateInput, uint> _steps;
	readonly ushort                         _minimum;

	public Migrate(Func<DbContext, IQueryable<TFrom>> query, IEntityProcessor<TFrom> processor)
		: this(new Total<TFrom>(query), new Pages<TFrom>(query, processor)) {}

	public Migrate(IStopAware<IWorkspaces, uint> total, IStopAware<MigrateInput, uint> steps)
		: this(total, steps, MinimumBatchSize.Default) {}

	public Migrate(IStopAware<IWorkspaces, uint> total, IStopAware<MigrateInput, uint> steps, ushort minimum)
	{
		_total   = total;
		_steps   = steps;
		_minimum = minimum;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var ((logger, definition, size), stop) = parameter;
		var total = await _total.Off(new(definition, stop));
		if (total > 0)
		{
			logger.LogInformation("{From} -> {To}: Starting with {Total} items...", A.Type<TFrom>(), A.Type<TTo>(),
			                      total);
			var watch     = Stopwatch.StartNew();
			var values    = new Values(_minimum, size ?? DefaultBatchSize.Default, total);
			var processed = await _steps.Off(new(new(logger, definition, values), stop));
			logger.LogInformation("{From} -> {To}: Completed all {Total} items in {Elapsed:mm\\:ss\\.fff} ({Rate:F1} entities/sec)",
			                      A.Type<TFrom>(), A.Type<TTo>(), processed, watch.Elapsed,
			                      processed / watch.Elapsed.TotalSeconds);
		}
		else
		{
			logger.LogInformation("{From} -> {To}: No rows found in source", A.Type<TFrom>(), A.Type<TTo>());
		}
	}
}