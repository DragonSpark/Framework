using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Content;

public interface IMigrator<T> : ISelect<MigrationInput<T>, T>;