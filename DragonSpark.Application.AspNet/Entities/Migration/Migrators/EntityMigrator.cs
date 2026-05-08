using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class EntityMigrator<TFrom, TTo> : EntityMigratorBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public EntityMigrator(DbContext source, DbContext destination) : this(source, destination, Map.Default) {}

	public EntityMigrator(DbContext source, DbContext destination, string name)
		: this(new(source, destination, name), Map.Default) {}

	public EntityMigrator(DbContext source, DbContext destination, IMap map) : this(new(source, destination), map) {}

	public EntityMigrator(Contexts<TFrom> contexts, IMap map) : base(contexts, map) {}

	public EntityMigrator(Contexts<TFrom> contexts, IEntityProcessor<TFrom> processor) : base(contexts, processor) {}
}