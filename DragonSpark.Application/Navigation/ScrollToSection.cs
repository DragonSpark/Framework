namespace DragonSpark.Application.Navigation;

public sealed class ScrollToSection : Text.Text
{
	public static ScrollToSection Default { get; } = new ScrollToSection();

	ScrollToSection() : base(nameof(ScrollToSection)) {}
}