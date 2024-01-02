namespace DragonSpark.Azure.Events;

public abstract record Message;
public abstract record Message<T>(T Subject) : Message;