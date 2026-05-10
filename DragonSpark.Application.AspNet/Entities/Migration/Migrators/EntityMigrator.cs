using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class EntityMigrator<TFrom, TTo> : EntityMigratorBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public EntityMigrator(DbContext source, DbContext destination) : this(source, destination, Map.Default) {}

	public EntityMigrator(DbContext source, DbContext destination, string name)
		: base(new(source, destination, name), new MapName(name)) {}

	public EntityMigrator(DbContext source, DbContext destination, IMap map) : this(new(source, destination), map) {}

	public EntityMigrator(Contexts<TFrom> contexts, IMap map) : base(contexts, map) {}

	public EntityMigrator(Contexts<TFrom> contexts, IEntityProcessor<TFrom> processor) : base(contexts, processor) {}
}

sealed class NamedEntityMigrator : EntityMigratorBase<Dictionary<string, object>, Dictionary<string, object>>
{
	public NamedEntityMigrator(Contexts<Dictionary<string, object>> contexts) 
		: base(contexts, new NamedEntityProcessor()) {}
}

// TODO V2

sealed class NamedEntityProcessor : IEntityProcessor<Dictionary<string, object>>
{
	public ValueTask Get(Stop<SourceInput<Dictionary<string, object>>> parameter)
	{
		// var ((logger, pageSize, dbContext, destination, queryable, total), stop) = parameter;
		return ValueTask.CompletedTask;
	}
}
sealed class MapName : IMap
{
	readonly string _name;

	public MapName(string name) => _name = name;

	public ValueTask Get(Stop<MapInput> parameter)
	{
		var ((from, to), _) = parameter;
		to.Context.Set<Dictionary<string, object>>(_name).Update(from.Entity.To<Dictionary<string, object>>());
		return ValueTask.CompletedTask;
	}
}