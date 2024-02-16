namespace DragonSpark.Azure.Messaging;

public record NumberMessage(uint Subject) : Message<uint>(Subject);