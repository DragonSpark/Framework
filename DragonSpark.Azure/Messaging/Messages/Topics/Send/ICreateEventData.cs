using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Send;

public interface ICreateEventData : ISelect<CreateEventDataInput, ServiceBusMessage>;