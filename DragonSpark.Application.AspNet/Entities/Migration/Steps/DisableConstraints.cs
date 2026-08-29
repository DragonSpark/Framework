using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class DisableConstraints : ExecuteStepBase
{
	public DisableConstraints(DatabaseFacade database) : base(database, DisableStatements.Default) {}
}