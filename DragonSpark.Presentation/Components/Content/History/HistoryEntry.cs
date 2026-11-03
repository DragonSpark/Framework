namespace DragonSpark.Presentation.Components.Content.History;

public readonly record struct HistoryEntry<T>(string Key, T? Result, DateTimeOffset When);