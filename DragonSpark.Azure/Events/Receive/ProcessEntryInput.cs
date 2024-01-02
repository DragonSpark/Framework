namespace DragonSpark.Azure.Events.Receive;

public readonly record struct ProcessEntryInput(object Message, Handlers Handlers);