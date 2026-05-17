using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

sealed class UpsertProcessors<TFrom, TTo> : IProcessors<TFrom> where TFrom : class where TTo : class
{
	public static UpsertProcessors<TFrom, TTo> Default { get; } = new();

	UpsertProcessors() {}

	public IEntityProcessor<TFrom> Get(ProcessorsInput<TFrom> parameter)
	{
		var (_, map) = parameter;
		return new UpsertEntities<TFrom, TTo>(map);
	}
}