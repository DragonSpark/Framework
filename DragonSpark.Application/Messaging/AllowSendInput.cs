namespace DragonSpark.Application.Messaging;

public readonly record struct AllowSendInput(string Address, string Subject, string Message);