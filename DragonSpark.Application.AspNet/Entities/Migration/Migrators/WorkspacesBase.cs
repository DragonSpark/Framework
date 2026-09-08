using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class WorkspacesBase<TFrom, TTo> : IWorkspaces where TFrom : DbContext where TTo : DbContext
{
	readonly INewContext<TFrom> _source;
	readonly INewContext<TTo>   _destination;

	protected WorkspacesBase(INewContext<TFrom> source, INewContext<TTo> destination, DbContext instance)
		: this(source, destination, instance.Model, instance.Database) {}

	// ReSharper disable once TooManyDependencies
	protected WorkspacesBase(INewContext<TFrom> source, INewContext<TTo> destination, IModel model,
	                         DatabaseFacade database)
	{
		_source      = source;
		_destination = destination;
		Database     = database;
		Model        = model;
	}

	public DatabaseFacade Database { get; }

	public IModel Model { get; }

	public Workspace Get() => new(_source.Get(), _destination.Get());
}