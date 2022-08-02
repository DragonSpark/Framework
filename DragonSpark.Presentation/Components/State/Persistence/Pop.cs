namespace DragonSpark.Presentation.Components.State.Persistence;

public readonly record struct Pop<T>(bool Success, T? Value);