using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;

sealed class Destination : IDestination
{
	public Destination(DbContext context) : this(context.Model, context.Database) {}

	public Destination(IModel model, DatabaseFacade database)
	{
		Model    = model;
		Database = database;
	}

	public IModel Model { get; }
	public DatabaseFacade Database { get; }
}