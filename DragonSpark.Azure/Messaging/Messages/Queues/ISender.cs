using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

public interface ISender : IStopAware<MessageInput>, ISelect<SendInput, ISend>, IStopAware<ServiceBusMessage>;