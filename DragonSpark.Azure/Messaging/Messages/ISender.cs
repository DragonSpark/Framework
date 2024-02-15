using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Azure.Messaging.Messages;

// TODO
public readonly record struct SendInput(TimeSpan? Life = null, TimeSpan? Visibility = null);
public interface ISender : IOperation<MessageInput>, ISelect<SendInput, ISend>, IOperation<ServiceBusMessage>;