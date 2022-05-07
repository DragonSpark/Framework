using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Dialogs;

public readonly record struct ConfirmRecord<T>(DialogParameter<T> Input, ICommand<ConfirmResult?> Assignment);