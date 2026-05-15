using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

sealed class ExceptionAwareEntityMigratorSelector : IEntityMigratorSelector
{
	readonly IEntityMigratorSelector _previous;

	public ExceptionAwareEntityMigratorSelector(IEntityMigratorSelector previous) => _previous = previous;

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
	{
		try
		{
			return _previous.Get(parameter);
		}
		catch (Exception e)
		{
			var (_, _, result) = parameter;
			throw new
				InvalidOperationException($"A problem was encountered while selecting a migrator for {result.From}", e);
		}
	}
}