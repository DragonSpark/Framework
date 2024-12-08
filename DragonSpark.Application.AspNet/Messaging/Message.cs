namespace DragonSpark.Application.Messaging;

public readonly record struct Message(string To, string Title, string Body);