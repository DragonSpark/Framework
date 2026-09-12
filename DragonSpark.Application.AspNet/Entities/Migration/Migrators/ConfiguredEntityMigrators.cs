using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class ConfiguredEntityMigrators : IEntityMigrators
{
	readonly IEntityMigrators       _previous;
	readonly Action<IWorkspaceDefinition> _configure;

	public ConfiguredEntityMigrators(IEntityMigrators previous, Action<IWorkspaceDefinition> configure)
	{
		_previous  = previous;
		_configure = configure;
	}

	public Array<IEntityMigrator> Get(IWorkspaceDefinition parameter)
	{
		_configure(parameter);
		return _previous.Get(parameter);
	}
}