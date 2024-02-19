using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class ServiceBusMessageHost : IAllocated<ServiceBusReceivedMessage>
{
	readonly IAllocated<string> _previous;

	protected ServiceBusMessageHost(IOperation<Guid> operation, ILoggerFactory factory)
		: this(new LoggedParsedIdentityTextHost(operation, factory)) {}

	public ServiceBusMessageHost(IAllocated<string> previous) => _previous = previous;

	public virtual Task Get(ServiceBusReceivedMessage parameter) => _previous.Get(parameter.Body.ToString());
}