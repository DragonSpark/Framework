namespace DragonSpark.Presentation.Components.Content.History;

public readonly record struct HistoryEntry(string Key, object? Result, DateTimeOffset When);