namespace DragonSpark.Azure.Events;

public record UserMessage(uint Subject) : Message<uint>(Subject);