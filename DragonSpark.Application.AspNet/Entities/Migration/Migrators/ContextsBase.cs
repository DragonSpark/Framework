using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class ContextsBase<T> : Result<DbContext>, IContexts where T : DbContext
{
	protected ContextsBase(INewContext<T> @new, DbContext instance) : this(@new, instance.Database, instance.Model) {}

	protected ContextsBase(INewContext<T> @new, DatabaseFacade database, IModel model) : base(@new.Get)
	{
		Database = database;
		Model    = model;
	}

	public DatabaseFacade Database { get; }

	public IModel Model { get; }
}