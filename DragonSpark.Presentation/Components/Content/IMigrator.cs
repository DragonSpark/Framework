using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content;

public interface IMigrator<T> : ICommand<MigrationInput<T>>;