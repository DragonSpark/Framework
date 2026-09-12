using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class PostEntityMigrator : IEntityMigrator
{
	readonly IEntityMigrator      _previous;
	readonly IWorkspaceDefinition _definition;
	readonly ushort               _page;

	public PostEntityMigrator(IEntityMigrator previous, IWorkspaceDefinition definition) 
		: this(previous, definition, DefaultBatchSize.Default) {}

	public PostEntityMigrator(IEntityMigrator previous, IWorkspaceDefinition definition, ushort page)
	{
		_previous   = previous;
		_definition = definition;
		_page       = page;
	}

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter)
	{
		var (subject, stop) = parameter;
		return _previous.Get(new EntityMigratorInput(subject.Logger, _definition, _page).Stop(stop));
	}

	public ValueTask Get(Stop<EntityMigratorInput> parameter) => ValueTask.CompletedTask;

	public EntityTypeMapping Get() => _previous.Get();
}