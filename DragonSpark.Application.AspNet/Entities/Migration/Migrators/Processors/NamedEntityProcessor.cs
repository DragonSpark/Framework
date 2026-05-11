using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

sealed class NamedEntityProcessor : EntityProcessorBase<Dictionary<string, object>, Dictionary<string, object>>
{
	public NamedEntityProcessor(IEntityType type)
		: base(Source<Dictionary<string, object>>.Default, new NamedDestination(type),
		       Save<Dictionary<string, object>>.Default) {}
}