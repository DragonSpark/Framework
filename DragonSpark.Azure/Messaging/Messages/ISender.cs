using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Messaging.Messages;

public interface ISender : IOperation<MessageInput>, ISelect<SendInput, ISend>, IOperation<ServiceBusMessage>;