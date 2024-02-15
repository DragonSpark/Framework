using Azure.Messaging.ServiceBus;
using DragonSpark.Azure.Messaging.Messages;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ContainsIntendedAudience = DragonSpark.Azure.Messaging.Messages.ContainsIntendedAudience;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class LoggedParsedIdentityTextHost : IAllocated<string>
{
	readonly IAllocated<string> _body;

	public LoggedParsedIdentityTextHost(IOperation<Guid> operation, ILoggerFactory factory)
		: this(new LoggingAwareProcessor(operation, factory)) {}

	protected LoggedParsedIdentityTextHost(IOperation<Guid> operation) : this(new ParsedIdentity(operation)) {}

	protected LoggedParsedIdentityTextHost(IAllocated<string> body) => _body = body;

	public Task Get(string parameter) => _body.Get(parameter);
}

// TODO

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