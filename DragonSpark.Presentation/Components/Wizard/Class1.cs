namespace DragonSpark.Presentation.Components.Wizard;

class Class1;

public sealed class WizardStepArgs
{
	public WizardStepArgs(int index, int active)
	{
		Index  = index;
		Active = index == active;
	}

	public int Index { get; }

	public bool Active { get; }
}

public sealed class WizardStepChangeEventArgs
{
	/// <summary />
	public WizardStepChangeEventArgs(int targetIndex, string targetLabel)
	{
		TargetIndex = targetIndex;
		TargetLabel = targetLabel;
	}

	/// <summary />
	public int TargetIndex { get; }

	/// <summary />
	public string TargetLabel { get; }

	/// <summary />
	public bool IsCancelled { get; set; }
}