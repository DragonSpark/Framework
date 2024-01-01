namespace DragonSpark.Azure.Events;

public readonly record struct ProcessEntryInput(object Message, Handlers Handlers);