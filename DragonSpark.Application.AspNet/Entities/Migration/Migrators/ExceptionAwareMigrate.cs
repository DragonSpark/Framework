using DragonSpark.Application.AspNet.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ExceptionAwareMigrate<TFrom, TTo> : IStopAware<EntityMigratorInput>
{
	readonly IStopAware<EntityMigratorInput> _previous;
	readonly ICondition<Exception>           _condition;

	public ExceptionAwareMigrate(IStopAware<EntityMigratorInput> previous) : this(previous, ShouldProcess.Default) {}

	public ExceptionAwareMigrate(IStopAware<EntityMigratorInput> previous, ICondition<Exception> condition)
	{
		_previous  = previous;
		_condition = condition;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		try
		{
			await _previous.On(parameter);
		}
		catch (Exception e) when (_condition.Get(e))
		{
			var ((logger, _, _), _) = parameter;
			logger.LogError(e, "A problem was encountered while processing the entities {From} -> {To}",
			                typeof(TFrom), typeof(TTo));
			throw;
		}
	}
}