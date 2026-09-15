using DragonSpark.Application.AspNet.Diagnostics;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

sealed class ExceptionAwareEntityProcessor<TFrom, TTo> : IEntityProcessor<TFrom>
{
	readonly IEntityProcessor<TFrom> _previous;
	readonly ICondition<Exception>   _process;

	public ExceptionAwareEntityProcessor(IEntityProcessor<TFrom> previous) : this(previous, ShouldProcess.Default) {}

	public ExceptionAwareEntityProcessor(IEntityProcessor<TFrom> previous, ICondition<Exception> process)
	{
		_previous = previous;
		_process  = process;
	}

	public async ValueTask Get(Stop<SourceInput<TFrom>> parameter)
	{
		try
		{
			await _previous.On(parameter);
		}
		catch (Exception e)
		{
			if (_process.Get(e))
			{
				var ((logger, _, _, _, _), _) = parameter;
				logger.LogError(e, "{From} -> {To} - A problem was encountered while mapping these entities",
				                typeof(TFrom), typeof(TTo));
			}

			throw;
		}
	}
}