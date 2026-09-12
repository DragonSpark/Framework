using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;

public sealed class Entities : IEntities
{
	readonly IWorkspaces  _workspaces;
	readonly IDestination _destination;

	public Entities(IWorkspaceDefinition definition, DbContext origin, IResult<Workspace> enhanced)
		: this(definition, origin, definition, enhanced) {}

	// ReSharper disable once TooManyDependencies
	public Entities(IWorkspaces workspaces, DbContext origin, IDestination destination, IResult<Workspace> enhanced)
	{
		_workspaces  = workspaces;
		_destination = destination;
		Origin       = origin;
		Enhanced     = enhanced;
	}

	public DbContext Origin { get; }
	public IResult<Workspace> Enhanced { get; }

	public Workspace Get() => _workspaces.Get();

	public DatabaseFacade Database => _destination.Database;

	public IModel Model => _destination.Model;
}