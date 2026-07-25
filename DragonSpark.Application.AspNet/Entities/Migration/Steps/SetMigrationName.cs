using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class SetMigrationName : IMigrationStep
{
	readonly DbContext                 _subject;
	readonly string                    _name;
	readonly ITable<DbContext, string> _store;

	public SetMigrationName(DbContext subject, string name) : this(subject, name, ContextName.Default) {}

	public SetMigrationName(DbContext subject, string name, ITable<DbContext, string> store)
	{
		_subject = subject;
		_name    = name;
		_store   = store;
	}

	public ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		_store.Assign(_subject, _name);
		return ValueTask.CompletedTask;
	}
}