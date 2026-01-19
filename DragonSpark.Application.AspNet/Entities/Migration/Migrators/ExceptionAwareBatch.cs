using DragonSpark.Compose;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ExceptionAwareBatch<TFrom, TTo> : IBatch<TFrom>
{
	readonly IBatch<TFrom> _previous;

	public ExceptionAwareBatch(IBatch<TFrom> previous) => _previous = previous;

	public void Execute(BatchInput<TFrom> parameter)
	{
		try
		{
			_previous.Execute(parameter);
		}
		catch (Exception e)
		{
			var (logger, _, _, _, _, _) = parameter;
			logger.LogError(e, "{From} -> {To} - A problem was encountered while mapping these entities", typeof(TFrom),
			                typeof(TTo));
			throw;
		}
	}
}