using Azure.Messaging.EventHubs;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Events.Send;

public interface IProducer : IOperation<EventData>;