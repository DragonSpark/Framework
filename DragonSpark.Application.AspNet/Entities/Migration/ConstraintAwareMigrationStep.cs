using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class ConstraintAwareMigrationStep : IMigrationStep
{
	readonly IMigrationStep _previous;
	readonly DatabaseFacade _facade;

	public ConstraintAwareMigrationStep(IMigrationStep previous, DatabaseFacade facade)
	{
		_previous = previous;
		_facade   = facade;
	}

	public void Execute(EntityMigratorInput parameter)
	{
		_facade.ExecuteSqlRaw("EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';");
		try
		{
			_previous.Execute(parameter);
		}
		finally
		{
			_facade.ExecuteSqlRaw("EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';");	
		}
	}
}