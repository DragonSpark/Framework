using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;

public class WorkspaceDefinitionBase<TFrom, TTo> : IWorkspaceDefinition where TFrom : DbContext where TTo : DbContext
{
	readonly INewContext<TFrom> _from;
	readonly INewContext<TTo>   _to;
	readonly IDestination       _destination;

	// ReSharper disable once TooManyDependencies
	protected WorkspaceDefinitionBase(INewContext<TFrom> from, INewContext<TTo> to, DbContext source,
	                                  DbContext destination)
		: this(from, to, source, new Destination(destination)) {}

	// ReSharper disable once TooManyDependencies
	protected WorkspaceDefinitionBase(INewContext<TFrom> from, INewContext<TTo> to, DbContext source,
	                                  IDestination destination)
	{
		Source       = source;
		_from        = from;
		_to          = to;
		_destination = destination;
	}

	public Workspace Get() => new(_from.Get(), _to.Get());

	public DbContext Source { get; }


	public DatabaseFacade Database => _destination.Database;

	public IModel Model => _destination.Model;
}