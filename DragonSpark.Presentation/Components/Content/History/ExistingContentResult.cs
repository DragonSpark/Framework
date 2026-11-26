namespace DragonSpark.Presentation.Components.Content.History;

public readonly record struct ExistingContentResult<T>(ComponentEntryKey Key, T? Content);