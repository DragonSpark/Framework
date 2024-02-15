namespace DragonSpark.Azure.Messaging.Events;

public abstract record Message;
public abstract record Message<T>(T Subject) : Message;