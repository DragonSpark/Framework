using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Messages;

public class Sender : Instance<ServiceBusSender>, ISender
{
	protected Sender(ServiceBusClient client, string name) : base(client.CreateSender(name)) {}
}