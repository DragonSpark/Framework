using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class ConfiguredEntityMigrators : IEntityMigrators
{
	readonly IEntityMigrators       _previous;
	readonly Action<MigrationInput> _configure;

	public ConfiguredEntityMigrators(IEntityMigrators previous, Action<MigrationInput> configure)
	{
		_previous  = previous;
		_configure = configure;
	}

	public Array<IEntityMigrator> Get(MigrationInput parameter)
	{
		_configure(parameter);
		return _previous.Get(parameter);
	}
}