namespace DragonSpark.Presentation.Environment.Browser.Window;

sealed class NewWindowFocusElement : CreateReference<NewWindowFocusElementInput>
{
	public static NewWindowFocusElement Default { get; } = new();

	NewWindowFocusElement() : base(nameof(NewWindowFocusElement)) {}
}