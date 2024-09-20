using System;

namespace DragonSpark.Presentation.Components.Wizard;

[Flags]
public enum WizardButtonsLocation : byte
{
	Top, Bottom, Both = Top | Bottom
}