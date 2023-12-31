namespace DragonSpark.Azure.Events;

public record Message<T>(T Subject);