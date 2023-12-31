using Azure.Core;
using Azure.Messaging.ServiceBus;
using DragonSpark.Azure.Data;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messages;

sealed class Client : Instance<ServiceBusClient>, IClient, IAsyncDisposable
{
	public Client(ServiceBusConfiguration settings) : this(settings.Namespace, settings.TransportType) {}

	public Client(string @namespace, ServiceBusTransportType type)
		: this(@namespace, new() { TransportType = type }, DefaultCredential.Default) {}

	public Client(string @namespace, ServiceBusClientOptions options, TokenCredential credential)
		: base(new(@namespace, credential, options)) {}

	public ValueTask DisposeAsync() => Get().DisposeAsync();
}