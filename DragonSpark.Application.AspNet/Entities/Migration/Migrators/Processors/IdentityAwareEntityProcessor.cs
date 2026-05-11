using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

sealed class IdentityAwareEntityProcessor<TFrom, TTo> : EntityProcessorBase<TFrom, TTo>
	where TFrom : class where TTo : class
{
	public IdentityAwareEntityProcessor(IMap map, IEntityType type)
		: base(new IdentityAwareSource<TFrom, TTo>(type), new New<TFrom, TTo>(map), Insert<TTo>.Default) {}
}