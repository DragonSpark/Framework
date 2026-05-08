using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		await _facade.ExecuteSqlRawAsync("EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';").Off();
		try
		{
			await _previous.Off(parameter);
		}
		finally
		{
			try
			{
				await _facade
				      .ExecuteSqlRawAsync("EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';")
				      .Off();
			}
			catch (Exception e)
			{
				parameter.Subject.Logger.LogError(e, "An exception occurred while re-applying constraints");
				throw;
			}
		}
	}
}