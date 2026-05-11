using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

sealed class UpsertEntities<TFrom, TTo> : EntityProcessorBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public UpsertEntities(IMap map) : this(Source<TFrom>.Default, map) {}

	public UpsertEntities(ISource<TFrom> source, IMap map)
		: base(source, new Update<TFrom, TTo>(map), Save<TTo>.Default) {}
}