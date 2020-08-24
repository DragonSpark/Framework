namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public sealed class DefaultDisplayNameValidator : RegularExpressionValidator
	{
		public static DefaultDisplayNameValidator Default { get; } = new DefaultDisplayNameValidator();

		DefaultDisplayNameValidator() : base(DisplayNameValidation.Default.Get(50)) {}
	}
}