using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class EntityMigrator<TFrom, TTo> : EntityMigratorBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public EntityMigrator(DbContext source, IModel destination) : this(source, destination, Map.Default) {}

	public EntityMigrator(DbContext source, IModel destination, IMap map) : this(new(source, destination), map) {}

	public EntityMigrator(Contexts<TFrom> contexts, IMap map) : base(contexts, map) {}

	public EntityMigrator(Contexts<TFrom> contexts, IEntityProcessor<TFrom> processor) : base(contexts, processor) {}
}