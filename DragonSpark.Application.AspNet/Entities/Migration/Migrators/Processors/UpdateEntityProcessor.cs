using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

public sealed class UpdateEntityProcessor<TFrom, TTo> : StopAware<SourceInput<TFrom>>, IEntityProcessor<TFrom>
	where TTo : class where TFrom : class
{
	public UpdateEntityProcessor(IMap map)
		: base(new ExceptionAwareEntityProcessor<TFrom, TTo>(new UpsertEntities<TFrom,TTo>(map))) {}
}