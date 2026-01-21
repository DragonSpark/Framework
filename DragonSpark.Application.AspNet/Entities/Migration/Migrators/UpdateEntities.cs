namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class UpdateEntities<TFrom, TTo> : EntityProcessorBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public UpdateEntities(IMap map) : base(new Update<TFrom, TTo>(map), Update<TTo>.Default) {}
}