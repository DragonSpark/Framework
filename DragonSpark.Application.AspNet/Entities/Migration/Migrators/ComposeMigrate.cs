using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ComposeMigrate<TFrom, TTo> : ISelect<ComposeMigrateInput<TFrom>, IStopAware<EntityMigratorInput>>
{
	public static ComposeMigrate<TFrom, TTo> Default { get; } = new();

	ComposeMigrate() {}

	public IStopAware<EntityMigratorInput> Get(ComposeMigrateInput<TFrom> parameter)
	{
		var (query, processor) = parameter;
		var steps  = new Steps<TFrom>(query, processor);
		var total  = new Total<TFrom>(query);
		var body   = new Migrate<TFrom, TTo>(total, steps);
		var result = new ExceptionAwareMigrate<TFrom, TTo>(body);
		return result;
	}
}