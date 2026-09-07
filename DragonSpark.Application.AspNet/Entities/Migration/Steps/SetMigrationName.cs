using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class SetMigrationName : IMigrationStep
{
	readonly string                 _name;
	readonly ITable<IModel, string> _store;

	public SetMigrationName(string name) : this(name, ContextName.Default) {}

	public SetMigrationName(string name, ITable<IModel, string> store)
	{
		_name    = name;
		_store   = store;
	}

	public ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var ((_, contexts, _), _) = parameter;
		using var destination = contexts.Get();
		_store.Assign(destination.Model, _name);
		return ValueTask.CompletedTask;
	}
}