using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class LoggingAwareMigration : IMigration
{
	readonly IMigration  _previous;
	readonly ILogger     _log;
	readonly IWorkspaces _workspaces;

	public LoggingAwareMigration(IMigration previous, ILogger<LoggingAwareMigration> log,
	                             IWorkspaceDefinition workspaces)
	{
		_previous   = previous;
		_log        = log;
		_workspaces = workspaces;
	}

	public async ValueTask Get(Stop<ushort> parameter)
	{
		{
			await using var workspaces = _workspaces.Get();
			var (from, to) = workspaces;
			_log.LogInformation("Starting Migration: {Source} -> {Destination}", from.GetType(), to.GetType());
		}

		var stopwatch = Stopwatch.StartNew();
		await _previous.Off(parameter);
		_log.LogInformation("Migration Completed: {Elapsed}", stopwatch.Elapsed);
	}
}