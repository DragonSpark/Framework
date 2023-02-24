using DragonSpark.Presentation.Environment.Browser;

namespace DragonSpark.Presentation.Components.Content.Sequences.Virtualization;

sealed class Initialize : CreateReference<InitializeInput>
{
	public static Initialize Default { get; } = new();

	Initialize() : base(nameof(Initialize).ToLower()) {}
}