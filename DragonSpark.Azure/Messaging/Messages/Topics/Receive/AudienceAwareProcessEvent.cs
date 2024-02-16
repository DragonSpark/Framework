using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection.Conditions;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public sealed class AudienceAwareProcessEvent : IAllocated<ProcessMessageEventArgs>
{
	readonly ProcessEvent                        _previous;
	readonly ICondition<ProcessMessageEventArgs> _condition;

	public AudienceAwareProcessEvent(ProcessEvent previous, ServiceBusConfiguration configuration)
		: this(previous, configuration.Audience) {}

	public AudienceAwareProcessEvent(ProcessEvent previous, string? audience)
		: this(previous, new ContainsIntendedAudience(audience)) {}

	public AudienceAwareProcessEvent(ProcessEvent previous, ICondition<ProcessMessageEventArgs> condition)
	{
		_previous  = previous;
		_condition = condition;
	}

	public Task Get(ProcessMessageEventArgs parameter)
		=> _condition.Get(parameter) ? _previous.Get(parameter) : Task.CompletedTask;
}