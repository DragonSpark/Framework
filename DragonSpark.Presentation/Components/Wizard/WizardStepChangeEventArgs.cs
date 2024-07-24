namespace DragonSpark.Presentation.Components.Wizard;

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