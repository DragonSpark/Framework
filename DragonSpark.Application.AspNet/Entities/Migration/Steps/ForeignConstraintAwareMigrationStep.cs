using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

/*
sealed class ForeignConstraintAwareMigrationStep : IMigrationStep
{
	readonly IMigrationStep              _previous;
	readonly IStopAware<ConstraintInput> _input;
	readonly IExecute<ConstraintInput>   _execute;

	public ForeignConstraintAwareMigrationStep(IMigrationStep previous, DatabaseFacade database)
		: this(previous, new ComposeConstraintInput(database), new ExecuteForeignConstraints(database)) {}

	public ForeignConstraintAwareMigrationStep(IMigrationStep previous, IStopAware<ConstraintInput> input,
	                                           IExecute<ConstraintInput> execute)
	{
		_previous = previous;
		_input    = input;
		_execute  = execute;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var (_, stop) = parameter;

		var subject  = await _input.Off(stop);
		var input    = subject.Stop(stop);
		var complete = await _execute.Off(input);
		try
		{
			await _previous.On(parameter);
		}
		finally
		{
			await complete.Off(input);
		}
	}
}
*/

sealed class DisableConstraints : ExecuteStepBase
{
	public DisableConstraints(DatabaseFacade database) : base(database, DisableStatements.Default) {}
}

sealed class EnableConstraints : ExecuteStepBase
{
	public EnableConstraints(DatabaseFacade database) : base(database, EnableStatements.Default) {}
}


class ExecuteStepBase : IMigrationStep
{
	readonly IStopAware<ConstraintInput> _input;
	readonly IExecution<ConstraintInput> _execute;

	protected ExecuteStepBase(DatabaseFacade database, ISelect<ConstraintInput, IEnumerable<string>> statements)
		: this(ConstraintInputs.Default.Then().Bind(database).Out(),
		       new Execution<ConstraintInput>(database, statements)) {}

	protected ExecuteStepBase(IStopAware<ConstraintInput> input, IExecution<ConstraintInput> execute)
	{
		_input   = input;
		_execute = execute;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var (_, stop) = parameter;
		var input = await _input.Off(stop);
		await _execute.Off(new(input, stop));
	}
}


sealed class ConstraintInputs : ReferenceStoring<DatabaseFacade, ConstraintInput>
{
	public static ConstraintInputs Default { get; } = new();

	ConstraintInputs() : base(ComposeConstraintInput.Default) {}
}

sealed class ComposeConstraintInput : IStopAware<DatabaseFacade, ConstraintInput>
{
	public static ComposeConstraintInput Default { get; } = new();

	ComposeConstraintInput() : this(ConcurrencyRowsQuery.Default, IndexesQuery.Default) {}

	readonly string _columns;
	readonly string _indexes;

	public ComposeConstraintInput(string columns, string indexes)
	{
		_columns = columns;
		_indexes = indexes;
	}

	public async ValueTask<ConstraintInput> Get(Stop<DatabaseFacade> parameter)
	{
		var (subject, stop) = parameter;
		var targets = await subject.SqlQueryRaw<IndexKey>(_columns).ToArrayAsync(stop).Off();
		var indexes = await subject.SqlQueryRaw<UniqueIndex>(_indexes).ToArrayAsync(stop).Off();
		return new(targets, indexes);
	}
}

sealed class IndexesQuery : Text.Text
{
	public static IndexesQuery Default { get; } = new();

	IndexesQuery()
		: base("""
		       SELECT
		           s.name AS SchemaName,
		           t.name AS TableName,
		           i.name AS IndexName,
		           i.is_unique_constraint AS IsUniqueConstraint,
		           ic.key_ordinal AS KeyOrdinal,
		           ic.is_descending_key AS IsDescending,
		           ic.is_included_column AS IsIncluded,
		           col.name AS ColumnName,
		           i.filter_definition AS FilterDefinition
		       FROM sys.indexes i
		       JOIN sys.index_columns ic 
		           ON ic.object_id = i.object_id 
		          AND ic.index_id = i.index_id
		       JOIN sys.columns col 
		           ON col.object_id = ic.object_id 
		          AND col.column_id = ic.column_id
		       JOIN sys.tables t 
		           ON t.object_id = i.object_id
		       JOIN sys.schemas s 
		           ON s.schema_id = t.schema_id
		       WHERE i.is_unique = 1
		         AND i.is_primary_key = 0
		       ORDER BY s.name, t.name, i.name, ic.key_ordinal;
		       """) {}
}

public sealed record UniqueIndex(
	string SchemaName,
	string TableName,
	string IndexName,
	bool IsUniqueConstraint,
	byte KeyOrdinal,
	bool IsDescending,
	bool IsIncluded,
	string ColumnName,
	string? FilterDefinition
);

public sealed record ConstraintInput(Array<IndexKey> Targets, Array<IGrouping<IndexKey, UniqueIndex>> Indexes)
{
	public ConstraintInput(Array<IndexKey> Targets, Array<UniqueIndex> Indexes)
		: this(Targets, Indexes.Open().GroupBy(i => new IndexKey(i.SchemaName, i.TableName, i.IndexName)).Result()) {}
}

/*sealed class ExecuteForeignConstraints : Execute<ConstraintInput>
{
	public ExecuteForeignConstraints(DatabaseFacade database)
		: base(database, StartStatements.Default, CompletedStatements.Default) {}
}*/

sealed class DisableStatements : ISelect<ConstraintInput, IEnumerable<string>>
{
	public static DisableStatements Default { get; } = new();

	DisableStatements() {}

	public IEnumerable<string> Get(ConstraintInput parameter)
	{
		var (targets, indexes) = parameter;
		yield return "EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';";
		foreach (var group in indexes.Open())
		{
			var (schema, table, indexName) = group.Key;
			var first = group.First();

			yield return first.IsUniqueConstraint
				             ? $"ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT [{indexName}]"
				             : $"DROP INDEX [{indexName}] ON [{schema}].[{table}]";
		}

		foreach (var (schema, table, column) in targets.Open())
		{
			yield return $"ALTER TABLE [{schema}].[{table}] DROP COLUMN [{column}]";
			yield return $"ALTER TABLE [{schema}].[{table}] ADD [{column}] VARBINARY(8) NULL";
			yield return
				$"UPDATE [{schema}].[{table}] SET [{column}] = 0x0000000000000000 WHERE [{column}] IS NULL";
		}
	}
}

sealed class EnableStatements : ISelect<ConstraintInput, IEnumerable<string>>
{
	public static EnableStatements Default { get; } = new();

	EnableStatements() {}

	public IEnumerable<string> Get(ConstraintInput parameter)
	{
		var (targets, indexes) = parameter;
		foreach (var (schema, table, column) in targets.Open())
		{
			yield return $"ALTER TABLE [{schema}].[{table}] DROP COLUMN [{column}]";
			yield return $"ALTER TABLE [{schema}].[{table}] ADD [{column}] ROWVERSION";
		}

		// 4. Recreate all unique indexes + constraints
		foreach (var group in indexes.Open())
		{
			var first = group.First();
			var (schema, table, indexName) = group.Key;

			var columns = string.Join(", ",
			                          group.Where(i => i.KeyOrdinal > 0)
			                               .OrderBy(i => i.KeyOrdinal)
			                               .Select(i => $"[{i.ColumnName}] {(i.IsDescending ? "DESC" : "ASC")}"));

			var included      = group.Where(i => i.IsIncluded).Select(i => $"[{i.ColumnName}]").ToList();
			var includeClause = included.Any() ? $" INCLUDE ({string.Join(", ", included)})" : string.Empty;
			var filter = first.FilterDefinition is { Length: > 0 }
				             ? $" WHERE {first.FilterDefinition}"
				             : string.Empty;

			yield return first.IsUniqueConstraint
				             ? $"ALTER TABLE [{schema}].[{table}] ADD CONSTRAINT [{indexName}] UNIQUE ({columns})"
				             : $"CREATE UNIQUE INDEX [{indexName}] ON [{schema}].[{table}] ({columns}){includeClause}{filter}";
		}

		yield return "EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';";
	}
}

/*public interface IExecute<T> : IStopAware<T, IExecution<T>>;*/
/*
public class Execute<T> : IExecute<T>
{
	readonly IExecution<T> _start, _complete;

	protected Execute(DatabaseFacade database, ISelect<T, IEnumerable<string>> start,
	                  ISelect<T, IEnumerable<string>> complete)
		: this(new Execution<T>(database, start), new Execution<T>(database, complete)) {}

	public Execute(IExecution<T> start, IExecution<T> complete)
	{
		_start    = start;
		_complete = complete;
	}

	public async ValueTask<IExecution<T>> Get(Stop<T> parameter)
	{
		await _start.Off(parameter);
		return _complete;
	}
}
*/

public interface IExecution<T> : DragonSpark.Model.Operations.Stop.IStopAware<T>;

sealed class Execution<T> : IExecution<T>
{
	readonly DatabaseFacade                  _database;
	readonly ISelect<T, IEnumerable<string>> _statements;

	public Execution(DatabaseFacade database, ISelect<T, IEnumerable<string>> statements)
	{
		_database   = database;
		_statements = statements;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (subject, stop) = parameter;
		foreach (var statement in _statements.Get(subject))
		{
			await _database.ExecuteSqlRawAsync(statement, stop).Off();
		}
	}
}