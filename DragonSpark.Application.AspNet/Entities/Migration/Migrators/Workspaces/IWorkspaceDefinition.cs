using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;

public interface IWorkspaceDefinition : IWorkspaces, IDestination
{
	DbContext Source { get; }
}