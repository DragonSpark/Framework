namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public sealed class DefaultDisplayNameValidator : RegularExpressionValidator
	{
		public static DefaultDisplayNameValidator Default { get; } = new DefaultDisplayNameValidator();

		DefaultDisplayNameValidator() : base(DisplayNamePattern.Default.Bounded().Get(new Bounds(1, 50))) {}
	}
}