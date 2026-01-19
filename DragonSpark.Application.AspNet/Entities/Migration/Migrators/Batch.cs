namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Batch<TFrom, TTo> : BatchBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public Batch(IMap map) : base(new ComposeBatch<TFrom, TTo>(map), UpsertBatch<TTo>.Default) {}
}