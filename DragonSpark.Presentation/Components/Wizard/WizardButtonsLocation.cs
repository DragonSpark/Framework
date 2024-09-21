using System;

namespace DragonSpark.Presentation.Components.Wizard;

[Flags]
public enum WizardButtonsLocation : byte
{
	Top = 1, Bottom = 2, Both = Top | Bottom
}