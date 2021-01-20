using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Dialogs
{
	public interface IConfirming<in T> : ISelecting<T, DialogResult> {}
}