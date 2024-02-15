namespace DragonSpark.Azure.Messaging.Events.Receive;

public readonly record struct ProcessEntryInput(object Message, Handlers Handlers);