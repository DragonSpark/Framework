using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Wizard;

public sealed record WizardButtonInput(string Text, EventCallback Action);

public sealed record WizardButtonInputs(WizardButtonInput Previous, WizardButtonInput Next, WizardButtonInput Complete);