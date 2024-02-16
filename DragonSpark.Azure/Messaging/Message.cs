namespace DragonSpark.Azure.Messaging;

public abstract record Message;
public abstract record Message<T>(T Subject) : Message;