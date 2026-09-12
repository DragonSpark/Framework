using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class NamedEntityMigrator : EntityMigratorBase<Dictionary<string, object>, Dictionary<string, object>>
{
	public NamedEntityMigrator(IEntityType type)
		: base(d => d.Set<Dictionary<string, object>>(type.Name).Exact(), new NamedEntityProcessor(type)) {}
}