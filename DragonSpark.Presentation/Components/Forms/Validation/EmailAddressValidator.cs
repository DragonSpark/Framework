namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public sealed class EmailAddressValidator : RegularExpressionValidator
	{
		public static EmailAddressValidator Default { get; } = new EmailAddressValidator();

		EmailAddressValidator() : base(EmailAddressExpression.Default) {}
	}
}