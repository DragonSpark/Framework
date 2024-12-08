namespace DragonSpark.Application.AspNet.Messaging;

public readonly record struct AllowSendInput(string Address, string Subject, string Message);