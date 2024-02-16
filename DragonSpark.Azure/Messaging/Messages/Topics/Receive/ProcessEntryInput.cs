namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public readonly record struct ProcessEntryInput(object Message, Handlers Handlers);