using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class ComposeConstraintInput : IStopAware<DatabaseFacade, ConstraintInput>
{
	public static ComposeConstraintInput Default { get; } = new();

	ComposeConstraintInput() : this(ConcurrencyRowsQuery.Default, IndexesQuery.Default) {}

	readonly string _columns, _indexes;

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