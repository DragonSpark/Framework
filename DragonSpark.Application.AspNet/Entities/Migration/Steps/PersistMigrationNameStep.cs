using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class PersistMigrationNameStep : IMigrationStep
{
	public static PersistMigrationNameStep Default { get; } = new();

	PersistMigrationNameStep() : this(FirstRun.Default, MarkRun.Default) {}

	readonly IStopAware<DbContext, bool> _first;
	readonly IStopAware<DbContext>       _mark;

	public PersistMigrationNameStep(IStopAware<DbContext, bool> first, IStopAware<DbContext> mark)
	{
		_first = first;
		_mark  = mark;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var ((_, destination, _), stop) = parameter;
		await using var context = destination.Get();
		var             input   = context.Destination.Stop(stop);
		if (await _first.Off(input))
		{
			await _mark.Off(input);
		}
	}
}