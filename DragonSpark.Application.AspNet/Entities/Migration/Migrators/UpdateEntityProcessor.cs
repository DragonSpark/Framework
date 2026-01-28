using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class UpdateEntityProcessor<TFrom, TTo> : StopAware<ProcessChangesInput<TFrom>>, IEntityProcessor<TFrom>
	where TTo : class where TFrom : class
{
	public UpdateEntityProcessor(IMap map)
		: base(new ExceptionAwareEntityProcessor<TFrom, TTo>(new UpdateEntities<TFrom, TTo>(map))) {}
}