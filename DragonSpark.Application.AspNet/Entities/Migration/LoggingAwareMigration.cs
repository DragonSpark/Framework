using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public class LoggingAwareMigration : IMigration
{
	readonly IMigration  _previous;
	readonly ILogger     _log;
	readonly IWorkspaces _workspaces;

	protected LoggingAwareMigration(IMigration previous, ILogger log, IWorkspaces workspaces)
	{
		_previous   = previous;
		_log        = log;
		_workspaces = workspaces;
	}

	public ValueTask Get(Stop<ushort> parameter) => Run(_previous, parameter);

	public ValueTask Get(CancellationToken parameter) => Run(_previous, parameter);

	async ValueTask Run<T>(ISelect<T, ValueTask> select, T parameter)
	{
		{
			await using var workspaces = _workspaces.Get();
			var (source, destination) = workspaces;
			_log.LogInformation("Starting Migration: {Source} ({DatabaseSource}) -> {Destination} ({DatabaseDestination})",
			                    source.GetType(), source.Database.GetConnectionString(),
			                    destination.GetType(), destination.Database.GetConnectionString());
		}

		Console.Write(@"🔧 Paused for Review. Press ESC to abort or any other key to continue...");
		var key = Console.ReadKey(true);
		Console.WriteLine();

		switch (key.Key)
		{
			case ConsoleKey.Escape:
				_log.LogWarning("Migration aborted by user");
				Environment.Exit(1);
				break;
		}

		var stopwatch = Stopwatch.StartNew();
		await select.Off(parameter);
		_log.LogInformation("Migration Completed: {Elapsed}", stopwatch.Elapsed);
	}
}