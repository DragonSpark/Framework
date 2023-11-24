namespace DragonSpark.Application.Navigation;

public sealed class ScrollToSection : Text.Text
{
	public static ScrollToSection Default { get; } = new();

	ScrollToSection() : base(nameof(ScrollToSection)) {}
}