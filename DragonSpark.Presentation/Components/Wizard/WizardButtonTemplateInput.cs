namespace DragonSpark.Presentation.Components.Wizard;

public sealed record WizardButtonTemplateInput(byte Value, bool Active, WizardButtonInput? Previous, WizardButtonInput? Next);