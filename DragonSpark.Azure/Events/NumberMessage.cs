namespace DragonSpark.Azure.Events;

public record NumberMessage(uint Subject) : Message<uint>(Subject);