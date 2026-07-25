using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class PersistMigrationNameStep : IMigrationStep
{
	readonly ISelect<CancellationToken, ValueTask<bool>> _first;
	readonly IStopAware                                  _mark;

	public PersistMigrationNameStep(DbContext context)
		: this(FirstRun.Default.Then().Bind(context).Get(), MarkRun.Default.Then().Bind(context).Out()) {}

	public PersistMigrationNameStep(ISelect<CancellationToken, ValueTask<bool>> first, IStopAware mark)
	{
		_first = first;
		_mark  = mark;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var (_, stop) = parameter;

		if (await _first.Off(stop))
		{
			await _mark.Off(stop);
		}
	}
}