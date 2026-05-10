using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class IdentityAwareEntityProcessor<TFrom, TTo> : EntityProcessorBase<TFrom, TTo>
	where TFrom : class where TTo : class
{
	public IdentityAwareEntityProcessor(IMap map, IEntityType type)
		: base(new IdentityAwareSource<TFrom, TTo>(type), new New<TFrom, TTo>(map), Insert<TTo>.Default) {}
}