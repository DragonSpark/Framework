namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public sealed class DisplayNamePattern : Expression
	{
		public static DisplayNamePattern Default { get; } = new DisplayNamePattern();

		DisplayNamePattern() : base("[a-zA-Z0-9- _]") {}
	}
}