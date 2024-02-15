using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection.Conditions;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public sealed class AudienceAwareProcessEvent : IAllocated<ProcessEventArgs>
{
	readonly ProcessEvent                 _previous;
	readonly ICondition<ProcessEventArgs> _condition;

	public AudienceAwareProcessEvent(ProcessEvent previous, EventHubConfiguration configuration)
		: this(previous, configuration.Audience) {}

	public AudienceAwareProcessEvent(ProcessEvent previous, string? audience)
		: this(previous, new ContainsIntendedAudience(audience)) {}

	public AudienceAwareProcessEvent(ProcessEvent previous, ICondition<ProcessEventArgs> condition)
	{
		_previous  = previous;
		_condition = condition;
	}

	public Task Get(ProcessEventArgs parameter)
		=> _condition.Get(parameter) ? _previous.Get(parameter) : Task.CompletedTask;
}

// TODO

public sealed class ContainsIntendedAudience : Condition<ProcessEventArgs>
{
	public ContainsIntendedAudience(string? audience)
		: base(new Messaging.Receive.ContainsIntendedAudience(audience).Get) {}
}