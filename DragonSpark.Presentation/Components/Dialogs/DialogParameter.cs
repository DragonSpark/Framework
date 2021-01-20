using Radzen;

namespace DragonSpark.Presentation.Components.Dialogs
{
	public static class DialogParameter
	{
		public static DialogParameter<T> From<T>(DialogService dialogs, T context) => new(dialogs, context);
	}

	public readonly struct DialogParameter<T>
	{
		public DialogParameter(DialogService dialogs, T context)
		{
			Dialogs = dialogs;
			Context = context;
		}

		public DialogService Dialogs { get; }

		public T Context { get; }
	}
}