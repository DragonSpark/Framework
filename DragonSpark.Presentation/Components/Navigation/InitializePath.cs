namespace DragonSpark.Presentation.Components.Navigation;

public sealed class InitializePath : ReturnPath
{
	public static InitializePath Default { get; } = new();

	InitializePath() : base("/Identity/Initialize") {}
}