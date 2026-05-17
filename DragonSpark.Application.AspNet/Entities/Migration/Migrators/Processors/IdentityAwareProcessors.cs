using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

sealed class IdentityAwareProcessors<TFrom, TTo> : IProcessors<TFrom> where TFrom : class where TTo : class
{
	public static IdentityAwareProcessors<TFrom, TTo> Default { get; } = new();

	IdentityAwareProcessors() : this(Sources<TFrom, TTo>.Default, UpsertProcessors<TFrom, TTo>.Default) {}

	readonly ISelect<Contexts<TFrom>, ISource<TFrom>?> _source;
	readonly IProcessors<TFrom>                        _previous;

	public IdentityAwareProcessors(ISelect<Contexts<TFrom>, ISource<TFrom>?> source, IProcessors<TFrom> previous)
	{
		_source   = source;
		_previous = previous;
	}

	public IEntityProcessor<TFrom> Get(ProcessorsInput<TFrom> parameter)
	{
		var (_, map) = parameter;
		var source = _source.Get(parameter.Contexts);
		return source is not null
			       ? new IdentityAwareEntityProcessor<TFrom, TTo>(source, map)
			       : _previous.Get(parameter);
	}
}