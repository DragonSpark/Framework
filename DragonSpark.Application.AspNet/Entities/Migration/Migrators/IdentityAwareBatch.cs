using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class IdentityAwareBatch<TFrom, TTo> : BatchBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public IdentityAwareBatch(IMap map, IEntityType type)
		: base(new IdentityAwareComposeBatch<TFrom, TTo>(map, type), InsertBatch<TTo>.Default) {}
}