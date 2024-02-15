namespace DragonSpark.Azure.Messaging.Events;

public record NumberMessage(uint Subject) : Message<uint>(Subject);