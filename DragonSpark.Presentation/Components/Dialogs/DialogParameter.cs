using Radzen;

namespace DragonSpark.Presentation.Components.Dialogs
{
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