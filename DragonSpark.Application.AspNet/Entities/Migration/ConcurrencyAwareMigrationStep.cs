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
			foreach (var (schema, table, column) in targets)
			{
				var drop = $"ALTER TABLE [{schema}].[{table}] DROP COLUMN [{column}]";
				var add  = $"ALTER TABLE [{schema}].[{table}] ADD [{column}] VARBINARY(8) NULL";
				var update =
					$"UPDATE [{schema}].[{table}] SET [{column}] = 0x0000000000000000 WHERE [{column}] IS NULL";
				await _database.ExecuteSqlRawAsync(drop).Off();
				await _database.ExecuteSqlRawAsync(add).Off();
				await _database.ExecuteSqlRawAsync(update).Off();
			}

			await _previous.On(parameter);
		}
		finally
		{
			foreach (var (schema, table, column) in targets)
			{
				var drop  = $"ALTER TABLE [{schema}].[{table}] DROP COLUMN [{column}]";
				var add   = $"ALTER TABLE [{schema}].[{table}] ADD [{column}] ROWVERSION";
				await _database.ExecuteSqlRawAsync(drop).Off();
				await _database.ExecuteSqlRawAsync(add).Off();
			}
		}
	}
}