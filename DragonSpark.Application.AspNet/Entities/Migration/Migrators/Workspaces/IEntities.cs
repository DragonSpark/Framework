using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;

public interface IEntities : IWorkspaces, IDestination
{
	DbContext Origin { get; }

	IResult<Workspace> Enhanced { get; }
}