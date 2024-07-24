namespace DragonSpark.Presentation.Components.Wizard;

public sealed class WizardStepArgs(int index, int active)
{
	public int Index { get; } = index;

	public bool Active { get; } = index == active;
}