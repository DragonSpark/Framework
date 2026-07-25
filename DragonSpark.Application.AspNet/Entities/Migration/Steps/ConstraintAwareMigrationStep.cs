using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public sealed class ConstraintAwareMigrationStep : IMigrationStep
{
	readonly IMigrationStep _disable;
	readonly IMigrationStep _previous;
	readonly IMigrationStep _enable;

	public ConstraintAwareMigrationStep(DatabaseFacade facade, IMigrationStep previous)
		: this(new DisableConstraints(facade), previous, new EnableConstraints(facade)) {}

	public ConstraintAwareMigrationStep(IMigrationStep disable, IMigrationStep previous, IMigrationStep enable)
	{
		_disable  = disable;
		_previous = previous;
		_enable   = enable;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		try
		{
			await _disable.Off(parameter);
			await _previous.Off(parameter);
		}
		finally
		{
			try
			{
				await _enable.Off(parameter);
			}
			catch (Exception e)
			{
				parameter.Subject.Logger.LogError(e, "An exception occurred while re-applying constraints");
				throw;
			}
		}
	}
}