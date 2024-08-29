using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Components.Eventing;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Time;

sealed class ClientTimeOffsetStore : IOperation<TimeSpan>, IResult<TimeSpan>, IConditionAware
{
	readonly IEventAggregator         _aggregator;
	readonly IMutationAware<TimeSpan> _previous;

	public ClientTimeOffsetStore(IEventAggregator aggregator) :
		this(aggregator, new VisitedAwareVariable<TimeSpan>()) {}

	public ClientTimeOffsetStore(IEventAggregator aggregator, IMutationAware<TimeSpan> previous)
		: this(aggregator, previous, previous.Condition) {}

	public ClientTimeOffsetStore(IEventAggregator aggregator, IMutationAware<TimeSpan> previous,
	                             ICondition<None> condition)
	{
		_aggregator = aggregator;
		_previous   = previous;
		Condition   = condition;
	}

	public TimeSpan Get() => _previous.Get();

	public ICondition<None> Condition { get; }

	public ValueTask Get(TimeSpan parameter)
	{
		_previous.Execute(parameter);
		return _aggregator.Publish(new ClientOffsetAssignedMessage(parameter)).ToOperation();
	}
}