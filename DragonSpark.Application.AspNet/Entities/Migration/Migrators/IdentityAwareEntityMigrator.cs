using DragonSpark.Compose;
using DragonSpark.Model.Operations;
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

	public EntityTypeMapping Get() => _previous.Get();

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => _previous.Get(parameter);

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => _previous.Get(parameter);

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var formatWith = _template.FormatWith("ON");
		await _database.ExecuteSqlRawAsync(formatWith, parameter).Off();

		try
		{
			await _previous.On(parameter);
		}
		finally
		{
			await _database.ExecuteSqlRawAsync(_template.FormatWith("OFF"), parameter).Off();
		}
	}
}