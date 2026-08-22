namespace DragonSpark.Presentation.Components.Content;

public readonly record struct MigrationInput<T>(T Source, T Destination);