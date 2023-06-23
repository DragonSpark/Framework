using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Presentation.Components.Dialogs;

public interface IConfirming<in T> : ISelecting<T, DialogResult> {}