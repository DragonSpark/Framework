using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ExceptionAwareEntityProcessor<TFrom, TTo> : IEntityProcessor<TFrom>
{
	readonly IEntityProcessor<TFrom> _previous;

	public ExceptionAwareEntityProcessor(IEntityProcessor<TFrom> previous) => _previous = previous;

	public async ValueTask Get(Stop<SourceInput<TFrom>> parameter)
	{
		try
		{
			await _previous.On(parameter);
		}
		catch (Exception e)
		{
			var ((logger, _, _, _, _, _), _) = parameter;
			logger.LogError(e, "{From} -> {To} - A problem was encountered while mapping these entities", typeof(TFrom),
			                typeof(TTo));
			throw;
		}
	}
}