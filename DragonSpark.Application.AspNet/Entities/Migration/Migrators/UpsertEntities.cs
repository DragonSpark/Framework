namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class UpsertEntities<TFrom, TTo> : EntityProcessorBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public UpsertEntities(IMap map) : base(new New<TFrom, TTo>(map), Upsert<TTo>.Default) {}
}