using Radzen;

namespace DragonSpark.Presentation.Components.Dialogs
{
	public readonly record struct DialogParameter<T>(DialogService Dialogs, T Context);
}