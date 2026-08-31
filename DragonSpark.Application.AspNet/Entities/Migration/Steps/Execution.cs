using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

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