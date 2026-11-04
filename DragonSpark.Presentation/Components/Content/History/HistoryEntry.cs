namespace DragonSpark.Presentation.Components.Content.History;

public readonly record struct HistoryEntry<T>(T? Result, DateTimeOffset When);