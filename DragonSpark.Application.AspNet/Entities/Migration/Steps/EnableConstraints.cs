using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class EnableConstraints : ExecuteStepBase
{
	public EnableConstraints(DatabaseFacade database) : base(database, EnableStatements.Default) {}
}