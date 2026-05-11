using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class NamedEntityMigrator : EntityMigratorBase<Dictionary<string, object>, Dictionary<string, object>>
{
	public NamedEntityMigrator(Contexts<Dictionary<string, object>> contexts, IEntityType type)
		: base(contexts, new NamedEntityProcessor(type)) {}
}