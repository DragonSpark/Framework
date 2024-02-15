using Azure.Messaging.ServiceBus;
using DragonSpark.Azure.Messaging.Messages;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class ServiceBusMessageHost : IAllocated<ServiceBusReceivedMessage>
{
	readonly IAllocated<string>                    _previous;
	readonly ICondition<ServiceBusReceivedMessage> _condition;

	protected ServiceBusMessageHost(IOperation<Guid> operation, ILoggerFactory factory,
	                                ServiceBusConfiguration configuration)
		: this(operation, factory, configuration.Audience) {}

	protected ServiceBusMessageHost(IOperation<Guid> operation, ILoggerFactory factory, string? audience)
		: this(new LoggedParsedIdentityTextHost(operation, factory), new ContainsIntendedAudience(audience)) {}

	public ServiceBusMessageHost(IAllocated<string> previous, ICondition<ServiceBusReceivedMessage> condition)
	{
		_previous  = previous;
		_condition = condition;
	}

	public virtual Task Get(ServiceBusReceivedMessage parameter)
		=> _condition.Get(parameter) ? _previous.Get(parameter.Body.ToString()) : Task.CompletedTask;
}