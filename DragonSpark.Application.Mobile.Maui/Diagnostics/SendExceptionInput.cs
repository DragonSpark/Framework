namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

public readonly record struct SendExceptionInput<T>(T Input, Exception Exception);