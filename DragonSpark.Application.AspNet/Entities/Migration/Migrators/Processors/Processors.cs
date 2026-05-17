using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

sealed class Processors<TFrom, TTo> : IProcessors<TFrom> where TFrom : class where TTo : class
{
	public static Processors<TFrom, TTo> Default { get; } = new();

	Processors() : this(IdentityAwareProcessors<TFrom, TTo>.Default) {}

	readonly IProcessors<TFrom> _previous;

	public Processors(IProcessors<TFrom> previous) => _previous = previous;

	public IEntityProcessor<TFrom> Get(ProcessorsInput<TFrom> parameter)
		=> new ExceptionAwareEntityProcessor<TFrom, TTo>(_previous.Get(parameter));
}