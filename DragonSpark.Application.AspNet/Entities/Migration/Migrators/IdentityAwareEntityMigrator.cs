using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class IdentityAwareEntityMigrator : IEntityMigrator
{
	readonly IEntityMigrator _previous;
	readonly DatabaseFacade  _database;
	readonly string          _template;

	public IdentityAwareEntityMigrator(IEntityMigrator previous, DbContext context, IEntityType type)
		: this(previous, context.Database, $"SET IDENTITY_INSERT [{type.GetSchema() ?? "dbo"}].[{type.GetTableName()}] {{0}}") {}

	public IdentityAwareEntityMigrator(IEntityMigrator previous, DatabaseFacade database, string template)
	{
		_previous = previous;
		_database = database;
		_template = template;
	}

	public void Execute(EntityMigratorInput parameter)
	{
		_database.ExecuteSqlRaw(_template.FormatWith("ON"));

		try
		{
			_previous.Execute(parameter);
		}
		finally
		{
			_database.ExecuteSqlRaw(_template.FormatWith("OFF"));
		}
	}

	public EntityTypeMapping Get() => _previous.Get();
}