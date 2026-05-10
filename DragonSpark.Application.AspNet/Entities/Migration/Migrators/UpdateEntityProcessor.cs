using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class UpdateEntityProcessor<TFrom, TTo> : StopAware<SourceInput<TFrom>>, IEntityProcessor<TFrom>
	where TTo : class where TFrom : class
{
	public UpdateEntityProcessor(IMap map)
		: base(new ExceptionAwareEntityProcessor<TFrom, TTo>(new UpsertEntities<TFrom,TTo>(map))) {}
}