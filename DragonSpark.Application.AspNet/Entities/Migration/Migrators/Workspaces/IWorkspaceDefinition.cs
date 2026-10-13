using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;

public interface IWorkspaceDefinition : IWorkspaces, IDestination
{
	IModel Source { get; }
}