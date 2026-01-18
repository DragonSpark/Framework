using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class ConstraintAwareMigration : IMigration
{
	readonly IMigration     _previous;
	readonly DatabaseFacade _facade;

	public ConstraintAwareMigration(IMigration previous, DatabaseFacade facade)
	{
		_previous = previous;
		_facade   = facade;
	}

	public void Execute(ushort parameter)
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