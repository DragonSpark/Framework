using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public sealed class AudienceAwareProcessEvent : IAllocated<ProcessEventArgs>
{
	readonly ProcessEvent                       _previous;
	readonly ICondition<ProcessEventArgs>       _condition;
	readonly ILogger<AudienceAwareProcessEvent> _logger;

	public AudienceAwareProcessEvent(ProcessEvent previous, EventHubConfiguration configuration,
	                                 ILogger<AudienceAwareProcessEvent> logger)
		: this(previous, configuration.Audience, logger) {}

	public AudienceAwareProcessEvent(ProcessEvent previous, string? audience, ILogger<AudienceAwareProcessEvent> logger)
		: this(previous, new ContainsIntendedAudience(audience), logger) {}

	public AudienceAwareProcessEvent(ProcessEvent previous, ICondition<ProcessEventArgs> condition,
	                                 ILogger<AudienceAwareProcessEvent> logger)
	{
		_previous  = previous;
		_condition = condition;
		_logger    = logger;
	}

	public Task Get(ProcessEventArgs parameter)
	{
		var b    = _condition.Get(parameter);
		var type = parameter.Data.Properties.TryGetValue(EventType.Default, out var t) ? t : null;
		_logger.LogInformation("{Hub}/{Partition}/{Offset} - {Type} - {Allowed}", parameter.Partition.EventHubName,
		                       parameter.Partition.PartitionId, parameter.Data.Offset,
							   type, b);
		return b ? _previous.Get(parameter) : Task.CompletedTask;
	}
}