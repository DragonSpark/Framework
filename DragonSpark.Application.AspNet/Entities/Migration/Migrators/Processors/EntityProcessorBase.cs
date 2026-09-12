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

	public async ValueTask Get(Stop<SourceInput<TFrom>> parameter)
	{
		var ((logger, entities, _, size, total), stop) = parameter;
		if (total > 0)
		{
			logger.LogInformation("{From} -> {To}: Starting with {Total} items...", A.Type<TFrom>(), A.Type<TTo>(),
			                      total);
			
			var watch    = Stopwatch.StartNew();
			var graph = 0u;
			var count    = 0u;

			await foreach (var page in _source.Get(parameter).AsAsyncEnumerable().Chunk(size).WithCancellation(stop))
			{
				// TODO:
				// await using var transaction = await workspaces.Database.BeginTransactionAsync(stop).Off();
				count += (uint)page.Length;
				logger.LogInformation("{From} -> {To}: Processing {Page} Items {Count}/{Total} ...", 
				                      A.Type<TFrom>(), A.Type<TTo>(), page.Length, count, total);
				await foreach (var workspace in
				               _destination.Get(new(new(logger, entities, page, total), stop)))
				{
					await using (workspace.ConfigureAwait(false))
					{
						var save = await _save.Off(new(new(logger, size, workspace, total), stop));
						graph += save;
					}
				}
			}


			logger.LogInformation("{From} -> {To}: Batch of {Count} processed in {Elapsed:mm\\:ss\\.fff} ({Rate:F1} entities/sec)",
			                      A.Type<TFrom>(), A.Type<TTo>(), graph, watch.Elapsed,
			                      graph / watch.Elapsed.TotalSeconds);
		}
		else
		{
			logger.LogInformation("{From} -> {To}: No rows found in source", A.Type<TFrom>(), A.Type<TTo>());
		}
	}
}
// TODO
/*sealed class TransactionAwareWorkspaces : IWorkspaces
{
	readonly IWorkspaces _workspaces;

	public TransactionAwareWorkspaces(IWorkspaces workspaces)
	{
		_workspaces = workspaces;
	}

	public Workspace Get() => _workspaces.Get();

	public DatabaseFacade Database => _workspaces.Database;

	public IModel Model => _workspaces.Model;
}*/