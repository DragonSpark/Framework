namespace DragonSpark.Application.AspNet.Messaging;

public readonly record struct Message(string To, string Title, string Body);