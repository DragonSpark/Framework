using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class ConcurrencyAwareMigrationStep : IMigrationStep
{
	readonly IMigrationStep _previous;
	readonly DatabaseFacade _database;
	readonly string         _query;

	public ConcurrencyAwareMigrationStep(IMigrationStep previous, DatabaseFacade database)
		: this(previous, database, ConcurrencyRowsQuery.Default) {}

	public ConcurrencyAwareMigrationStep(IMigrationStep previous, DatabaseFacade database, string query)
	{
		_previous = previous;
		_database = database;
		_query    = query;
	}

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var targets = await _database.SqlQueryRaw<TimestampTarget>(_query).ToArrayAsync(parameter).Off();

		try
		{
			foreach (var x in targets)
			{
				var drop = $"ALTER TABLE [{x.Schema}].[{x.Table}] DROP COLUMN [{x.Column}]";
				var add  = $"ALTER TABLE [{x.Schema}].[{x.Table}] ADD [{x.Column}] VARBINARY(8) NULL";
				await _database.ExecuteSqlRawAsync(drop).Off();
				await _database.ExecuteSqlRawAsync(add).Off();
			}

			await _previous.On(parameter);
		}
		finally
		{
			foreach (var target in targets)
			{
				var drop = $"ALTER TABLE [{target.Schema}].[{target.Table}] DROP COLUMN [{target.Column}]";
				var add  = $"ALTER TABLE [{target.Schema}].[{target.Table}] ADD [{target.Column}] ROWVERSION";
				await _database.ExecuteSqlRawAsync(drop).Off();
				await _database.ExecuteSqlRawAsync(add).Off();
			}
		}
	}
}