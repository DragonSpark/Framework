using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;

public sealed class Entities : Result<Workspace>, IEntities
{
	readonly IDestination _destination;

	public Entities(IWorkspaceDefinition definition) : this(definition, definition) {}

	public Entities(IWorkspaces workspaces, IDestination destination) : base(workspaces) => _destination = destination;

	public DatabaseFacade Database => _destination.Database;

	public IModel Model => _destination.Model;
}