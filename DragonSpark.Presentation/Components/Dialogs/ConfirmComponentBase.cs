namespace DragonSpark.Presentation.Components.Dialogs
{
	public abstract class ConfirmComponentBase : ComponentBase, IDialogResultAware
	{
		protected abstract ConfirmResult Result { get; }

		public DialogResult Get() => Result.Result ?? DialogResult.Other;
	}
}